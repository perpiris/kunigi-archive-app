using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
using KunigiArchive.Web.ViewModels.Game;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

[Route("games")]
public class GameController : Controller
{
    private readonly IGameService _gameService;
    private readonly ITeamService _teamService;

    public GameController(IGameService gameService, ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(gameService);
        ArgumentNullException.ThrowIfNull(teamService);
        
        _gameService = gameService;
        _teamService = teamService;
    }

    public async Task<IActionResult> Index(
        int pageNumber = 1, 
        int pageSize = 8, 
        string sortBy = "year",
        bool ascending = false,
        string? searchTerm = null)
    {
        var data = 
            await _gameService.GetPaginatedMasterGamesAsync(pageNumber, pageSize, false, sortBy, ascending, searchTerm);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SortBy = sortBy;
        ViewBag.Ascending = ascending;

        var viewModel = data.MapToPaginatedMasterGameDetailsViewModel();
        return View(viewModel);
    }

    [HttpGet("manage")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Manage(
        int pageNumber = 1, 
        int pageSize = 8, 
        string sortBy = "year",
        bool ascending = false,
        string? searchTerm = null)
    {
        var data = 
            await _gameService.GetPaginatedMasterGamesAsync(pageNumber, pageSize, true, sortBy, ascending, searchTerm);

        ViewBag.SearchTerm = searchTerm;
        ViewBag.SortBy = sortBy;
        ViewBag.Ascending = ascending;

        var viewModel = data.MapToPaginatedMasterGameDetailsViewModel();
        return View(viewModel);
    }
    
    [HttpGet("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create()
    {
        var viewModel = new MasterGameCreateViewModel();
        await PrepareMasterGameCreateViewModelAsync(viewModel);
        return View(viewModel);
    }
    
    [HttpPost("create")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(MasterGameCreateViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            await PrepareMasterGameCreateViewModelAsync(viewModel);
            return View(viewModel);
        }

        var result = await _gameService.CreateMasterGameAsync(viewModel.MapToCreateRequest(), ModelState);
        if (!result.IsSuccess)
        {
            await PrepareMasterGameCreateViewModelAsync(viewModel);
            return View(viewModel);
        }

        TempData["success-alert"] = "Το παιχνίδι δημιουργήθηκε επιτυχώς.";
        return RedirectToAction(nameof(Manage));
    }
    
    private async Task PrepareMasterGameCreateViewModelAsync(MasterGameCreateViewModel viewModel)
    {
        var teamSelectList = (await _teamService.GetTeamSelectListAsync()).ToList();
        
        viewModel.HostTeamList = teamSelectList;
        viewModel.WinnerTeamList = teamSelectList;
    }
}