using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
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
        string sortBy = "name",
        bool ascending = true)
    {
        var data = 
            await _teamService.GetPaginatedTeamsAsync(pageNumber, pageSize, false, sortBy, ascending);

        ViewBag.SortBy = sortBy;
        ViewBag.Ascending = ascending;

        var viewModel = data.ToPaginatedViewModel();
        return View(viewModel);
    }

    [HttpGet("manage")]
    public async Task<IActionResult> Manage(
        int pageNumber = 1, 
        int pageSize = 8, 
        string sortBy = "name",
        bool ascending = true)
    {
        var data = 
            await _teamService.GetPaginatedTeamsAsync(pageNumber, pageSize, true, sortBy, ascending);

        ViewBag.SortBy = sortBy;
        ViewBag.Ascending = ascending;

        var viewModel = data.ToPaginatedViewModel();
        return View(viewModel);
    }
}