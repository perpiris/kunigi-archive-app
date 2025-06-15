using System.ComponentModel.DataAnnotations;

namespace KunigiArchive.Web.ViewModels.Account;

public class AccountSettingsViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Current password")]
    public string? OldPassword { get; set; }

    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Confirm new password")]
    [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public string? ConfirmNewPassword { get; set; }
}