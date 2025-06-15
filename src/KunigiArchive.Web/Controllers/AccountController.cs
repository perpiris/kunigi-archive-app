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

    public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
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

    [HttpPost("settings")]
    public async Task<IActionResult> AccountSettings(AccountSettingsViewModel viewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        var user = await _userManager.GetUserAsync(User);
        if (user is null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }
        
        var successMessages = new List<string>();

        if (viewModel.Email != user.Email)
        {
            var emailResult = await _accountService.ChangeEmailAsync(user.Id, viewModel.Email, ModelState);
            if (emailResult.IsSuccess)
            {
                successMessages.Add("Your email has been changed successfully.");
            }
        }
        
        if (!string.IsNullOrEmpty(viewModel.NewPassword))
        {
            if (string.IsNullOrEmpty(viewModel.OldPassword))
            {
                ModelState.AddModelError(nameof(viewModel.OldPassword), "Current password is required to set a new password.");
            }
            else
            {
                var passwordResult = await _accountService.UpdatePasswordAsync(user.Id, viewModel.OldPassword, viewModel.NewPassword, ModelState);
                if (passwordResult.IsSuccess)
                {
                    successMessages.Add("Your password has been updated successfully.");
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }
        
        TempData["success-alerts"] = successMessages;
        return RedirectToAction(nameof(AccountSettings));
    }
}