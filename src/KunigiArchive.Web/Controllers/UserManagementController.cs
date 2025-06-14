using KunigiArchive.Application.Services;
using KunigiArchive.Web.ViewModels.UserManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KunigiArchive.Web.Controllers;

[Route("user-management")]
[Authorize(Roles = "Admin")]
public class UserManagementController : Controller
{
    private readonly IAccountService _accountService;

    public UserManagementController(IAccountService accountService)
    {
        ArgumentNullException.ThrowIfNull(accountService);
        
        _accountService = accountService;
    }
    
    public async Task<IActionResult> Index(
        int pageNumber = 1, 
        int pageSize = 8,
        string sortBy = "name")
    {
        
        
        return View();
    }
    
    [HttpGet("create")]
    public IActionResult CreateUser()
    {
        var viewModel = new UserCreateViewModel();
        return View(viewModel);
    }
    
    // [HttpPost("create")]
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