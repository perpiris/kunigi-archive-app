using System.Security.Claims;
using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

public class HomeController : Controller
{
    private readonly ITeamService _teamService;

    public HomeController(ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(teamService);
        
        _teamService = teamService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet("dashboard")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Dashboard()
    {
        if (User.IsInRole("Manager"))
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var managedTeamList = await _teamService.GetManagerTeamsAsync(userIdString!);
            ViewBag.TeamList = managedTeamList.Select(x => x.MapToDetailsViewModel());
        }
        
        return View();
    }
    
    [HttpGet("error")]
    public IActionResult Error()
    {
        Response.StatusCode = StatusCodes.Status500InternalServerError;
        return View();
    }

    [HttpGet("not-found")]
    public new IActionResult NotFound()
    {
        Response.StatusCode = StatusCodes.Status404NotFound;
        return View();
    }
    
    [HttpGet("access-denied")]
    public IActionResult AccessDenied()
    {
        Response.StatusCode = StatusCodes.Status403Forbidden;
        return View();
    }
}
