using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KunigiArchive.Web.ViewModels.Authentication;

public class LoginViewModel
{
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    [EmailAddress(ErrorMessage = "Το email δεν είναι έγκυρο")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    [DataType(DataType.Password)]
    [DisplayName("Κωδικός")]
    public required string Password { get; set; }
}