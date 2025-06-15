using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KunigiArchive.Web.ViewModels.UserManagement;

public class UserCreateViewModel
{
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    [EmailAddress(ErrorMessage = "Το email δεν είναι έγκυρο")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Παρακαλώ επιλέξτε έναν ρόλο.")]
    [Display(Name = "Ρόλος")]
    public string SelectedRole { get; set; } =  string.Empty;
    
    [Display(Name = "Ομάδα")]
    public long? SelectedTeamId { get; set; }

    public IEnumerable<SelectListItem> RolesList { get; set; } = new List<SelectListItem>();
    
    public IEnumerable<SelectListItem> TeamList { get; set; } = new List<SelectListItem>();
}