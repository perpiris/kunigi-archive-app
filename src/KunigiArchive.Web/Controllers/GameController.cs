using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

[Route("games")]
public class GameController : Controller
{
    private readonly IGameService _gameService;

    public GameController(IGameService gameService)
    {
        ArgumentNullException.ThrowIfNull(gameService);
        
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
}