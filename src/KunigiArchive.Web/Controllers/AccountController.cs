using KunigiArchive.Application.Services;
using KunigiArchive.Web.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KunigiArchive.Domain.Entities;

namespace KunigiArchive.Web.Controllers;
 

[Authorize]
[Route("account")]
public class AccountController : Controller
{
    private readonly IAccountService _accountService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(
        IAccountService accountService, 
        UserManager<ApplicationUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(accountService);
        ArgumentNullException.ThrowIfNull(userManager);

        _accountService = accountService;
        _userManager = userManager;
    }

    [HttpGet("settings")]
    public async Task<IActionResult> AccountSettings()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var viewModel = new AccountSettingsViewModel
        {
            Email = user.Email ?? string.Empty
        };

        return View(viewModel);
    }

    [HttpPost("settings/email")]
    public async Task<IActionResult> ChangeEmail(AccountSettingsViewModel viewModel)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        ModelState.Remove(nameof(viewModel.OldPassword));
        ModelState.Remove(nameof(viewModel.NewPassword));
        ModelState.Remove(nameof(viewModel.ConfirmNewPassword));

        if (!ModelState.IsValid)
        {
            return View(nameof(AccountSettings), viewModel);
        }

        var emailResult = await _accountService.ChangeEmailAsync(user.Id, viewModel.Email, ModelState);
        if (emailResult.IsSuccess)
        {
            TempData["success-alert"] = "Your email has been changed successfully.";
            return RedirectToAction(nameof(AccountSettings));
        }

        return View(nameof(AccountSettings), viewModel);
    }

    [HttpPost("settings/password")]
    public async Task<IActionResult> ChangePassword(AccountSettingsViewModel viewModel)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
        
        viewModel.Email = user.Email ?? string.Empty;
        ModelState.Remove(nameof(viewModel.Email));
        
        if (!ModelState.IsValid)
        {
            return View(nameof(AccountSettings), viewModel);
        }

        var passwordResult = await _accountService.UpdatePasswordAsync(user.Id, viewModel.OldPassword!, viewModel.NewPassword!, ModelState);
        if (passwordResult.IsSuccess)
        {
            TempData["success-alert"] = "Your password has been updated successfully.";
            return RedirectToAction(nameof(AccountSettings));
        }

        return View(nameof(AccountSettings), viewModel);
    }
}