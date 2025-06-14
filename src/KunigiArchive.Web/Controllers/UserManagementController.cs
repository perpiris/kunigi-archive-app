using KunigiArchive.Application.Services;
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
}