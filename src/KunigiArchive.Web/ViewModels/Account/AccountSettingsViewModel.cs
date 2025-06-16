using System.ComponentModel.DataAnnotations;

namespace KunigiArchive.Web.ViewModels.Account;

public class AccountSettingsViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public required string Email { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Κωδικός")]
    public string? OldPassword { get; set; }

    [StringLength(100, ErrorMessage = "Ο {0} πρέπει να είναι μεταξύ {2} και μέγιστο {1} χαρακτήρες.", MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Νέος Κωδικός")]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Display(Name = "Επαλήθευση νέου κωδικού")]
    [Compare("NewPassword", ErrorMessage = "Η επαλήθευση κωδικού δεν ταιριάζει με τον νέο κωδικό.")]
    public string? ConfirmNewPassword { get; set; }
}