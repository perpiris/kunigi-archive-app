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

    public GameController(IGameService gameService, ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(gameService);
        ArgumentNullException.ThrowIfNull(teamService);
        
        _gameService = gameService;
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

        var viewModel = data.MapToPaginatedViewModel();
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

        var viewModel = data.MapToPaginatedViewModel();
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
        var (hostTeams, winnerTeams, gameTypes) = await _gameService.GetCreateMasterGameSelectListsAsync();
        
        viewModel.HostTeamList = hostTeams;
        viewModel.WinnerTeamList = winnerTeams;
        viewModel.GameTypeList = gameTypes;
    }
}