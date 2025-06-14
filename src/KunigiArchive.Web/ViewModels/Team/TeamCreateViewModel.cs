using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KunigiArchive.Web.ViewModels.Team;

public class TeamCreateViewModel
{
    [DisplayName("Όνομα Ομάδας")]
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    [StringLength(150)]
    public string Name { get; set; } = null!;

    [DisplayName("Ενεργή")] public bool IsActive { get; set; } = true;
}