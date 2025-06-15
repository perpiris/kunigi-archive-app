using KunigiArchive.Application.Services;
using KunigiArchive.Web.Mappings;
using KunigiArchive.Web.ViewModels.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KunigiArchive.Web.Controllers;

[Route("user-management")]
[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly IAccountService _accountService;
    private readonly ITeamService _teamService;

    public UserManagementController(IAccountService accountService, ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(accountService);
        ArgumentNullException.ThrowIfNull(teamService);
        
        _accountService = accountService;
        _teamService = teamService;
    }
    
    public async Task<IActionResult> Manage(
        int pageNumber = 1, 
        int pageSize = 8,
        string sortBy = "name")
    {
        var data = await _accountService.GetPaginatedUsersAsync(pageNumber, pageSize, sortBy);
        
        var viewModel = data.MapToPaginatedViewModel();
        return View(viewModel);
    }
    
    [HttpGet("create-user")]
    public async Task<IActionResult> CreateUser()
    {
        var teamList = await _teamService.GetAllTeamsAsync();
        var viewModel = new UserCreateViewModel
        {
            RolesList = new List<SelectListItem>
            {
                new() { Value = "Admin", Text = "Admin" },
                new() { Value = "Manager", Text = "Manager" }
            },
            TeamList = teamList.Select(team => new SelectListItem
            {
                Value = team.TeamId.ToString(),
                Text = team.Name
            })
        };

        return View(viewModel);
    }
    
    // [HttpPost("create-user")]
    // public async Task<IActionResult> CreateUser(UserCreateViewModel viewModel)
    // {
    //     if (!ModelState.IsValid)
    //     {
    //         return View(viewModel);
    //     }
    //     
    //     var result = await _accountService.CreateUserAsync(viewModel.ToCreateRequest(), ModelState);
    //     if (!result.IsSuccess)
    //     {
    //         return View(viewModel);
    //     }
    //     
    //     TempData["success-alert"] = "Ο χρήστης δημιουργήθηκε επιτυχώς.";
    //     return RedirectToAction(nameof(Index));
    // }
}