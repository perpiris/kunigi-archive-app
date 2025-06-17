using System.Security.Claims;
using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
using KunigiArchive.Web.ViewModels.Team;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

[Route("teams")]
public class TeamController : Controller
{
    private readonly ITeamService _teamService;

    public TeamController(ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(teamService);

        _teamService = teamService;
    }

    public async Task<IActionResult> Index(
        int pageNumber = 1, 
        int pageSize = 8, 
        string? searchTerm = null)
    {
        var data = 
            await _teamService.GetPaginatedTeamsAsync(pageNumber, pageSize, false, searchTerm);

        ViewBag.SearchTerm = searchTerm;

        var viewModel = data.MapToPaginatedViewModel();
        return View(viewModel);
    }

    [HttpGet("manage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Manage(
        int pageNumber = 1, 
        int pageSize = 8, 
        string? searchTerm = null)
    {
        var data = 
            await _teamService.GetPaginatedTeamsAsync(pageNumber, pageSize, true, searchTerm);

        ViewBag.SearchTerm = searchTerm;

        var viewModel = data.MapToPaginatedViewModel();
        return View(viewModel);
    }
    
    [HttpGet("{idOrSlug}")]
    public async Task<IActionResult> Details(string idOrSlug)
    {
        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, true);

        if (data is null)
        {
            return RedirectToAction("NotFound", "Home");
        }

        var viewModel = data.MapToDetailsViewModel();
        return View(viewModel);
    }
    
    [HttpGet("{idOrSlug}/actions")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Actions(string idOrSlug)
    {
        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        if (!User.IsInRole("Admin") && !await _teamService.CanUserAccessTeam(userId, idOrSlug))
        {
            TempData["warning-alert"] = "Δεν έχετε δικαίωμα επεξεργασίας αυτής της ομάδας.";
            return RedirectToAction("Dashboard", "Home");
        }

        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, false);
        if (data is null)
        {
            return RedirectToAction("NotFound", "Home");
        }

        var viewModel = data.MapToDetailsViewModel();
        return View(viewModel);
    }
    
    [HttpGet("create")]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        var viewModel = new TeamCreateViewModel();
        return View(viewModel);
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(TeamCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var result = await _teamService.CreateTeamAsync(viewModel.MapToCreateRequest(), ModelState);
        if (!result.IsSuccess)
        {
            return View(viewModel);
        }

        TempData["success-alert"] = "Η ομάδα δημιουργήθηκε επιτυχώς.";
        return RedirectToAction(nameof(Manage));
    }
    
    [HttpGet("{idOrSlug}/edit")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Edit(string idOrSlug)
    {
        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, false);
        if (data is null)
        {
            TempData["error-alert"] = "Η ομάδα εδεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        if (!User.IsInRole("Admin") && !await _teamService.CanUserAccessTeam(userId, idOrSlug))
        {
            TempData["warning-alert"] = "Δεν έχετε δικαίωμα επεξεργασίας αυτής της ομάδας.";
            return RedirectToAction("Dashboard", "Home");
        }

        var viewModel = data.MapToEditViewModel();
        return View(viewModel);
    }

    [HttpPost("{idOrSlug}/edit")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Edit(string idOrSlug, TeamEditViewModel viewModel, IFormFile? image)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        if (string.IsNullOrEmpty(idOrSlug))
        {
            TempData["error-alert"] = "Η ομάδα εδεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        if (!User.IsInRole("Admin") && !await _teamService.CanUserAccessTeam(userId, idOrSlug))
        {
            TempData["warning-alert"] = "Δεν έχετε δικαίωμα επεξεργασίας αυτής της ομάδας.";
            return RedirectToAction("Dashboard", "Home");
        }

        var result = await _teamService.EditTeamAsync(viewModel.MapToEditRequest(), image, ModelState);
        if (!result.IsSuccess)
        {
            TempData["error-alert"] = result.Message;
            return View(viewModel);
        }

        TempData["success-alert"] = "Η ομάδα επεξεργάστηκε επιτυχώς.";
        return RedirectToAction(nameof(Actions), new { idOrSlug });
    }
    
    [HttpGet("{idOrSlug}/edit-managers")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> EditManagers(string idOrSlug)
    {
        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, false);
        if (data is null)
        {
            TempData["error-alert"] = "Η ομάδα δεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var teamWithManagers = await _teamService.GetTeamWithManagersAsync(idOrSlug);
        if (teamWithManagers is null)
        {
            TempData["error-alert"] = "Η ομάδα δεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var viewModel = teamWithManagers.MapToTeamManagerDetailsViewModel();
        return View(viewModel);
    }

    [HttpPost("{idOrSlug}/add-manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddManager(string idOrSlug, [FromForm] long userId)
    {
        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, false);
        if (data is null)
        {
            TempData["error-alert"] = "Η ομάδα δεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var result = await _teamService.AddTeamManagerAsync(idOrSlug, userId);
        if (result.IsSuccess)
        {
            TempData["success-alert"] = "Ο διαχειριστής προστέθηκε με επιτυχία.";
        }

        return RedirectToAction("EditManagers", new { idOrSlug });
    }

    [HttpPost("{idOrSlug}/remove-manager")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveManager(string idOrSlug, [FromForm] long userId)
    {
        var data = await _teamService.GetTeamByIdOrSlugAsync(idOrSlug, false);
        if (data is null)
        {
            TempData["error-alert"] = "Η ομάδα δεν βρέθηκε.";
            return RedirectToAction("NotFound", "Home");
        }

        var result = await _teamService.RemoveTeamManagerAsync(idOrSlug, userId);
        if (result.IsSuccess)
        {
            TempData["success-alert"] = "Ο διαχειριστής αφαιρέθηκε με επιτυχία.";
        }

        return RedirectToAction("EditManagers", new { idOrSlug });
    }
}