using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KunigiArchive.Web.ViewModels.Game;

public class MasterGameCreateViewModel
{
    [DisplayName("Τίτλος")]
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    [StringLength(500)]
    public string SubTitle { get; set; } = null!;

    [DisplayName("Έτος")]
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    public int? Year { get; set; }

    [DisplayName("Σειρά")]
    [Required(ErrorMessage = "Το πεδίο απαιτείται.")]
    public int? Order { get; set; }

    [DisplayName("Διοργανώτρια Ομάδα")]
    [Required(ErrorMessage = "Παρακαλώ επιλέξτε διοργανώτρια ομάδα.")]
    public long HostTeamId { get; set; }

    [DisplayName("Νικήτρια Ομάδα")]
    [Required(ErrorMessage = "Παρακαλώ επιλέξτε νικήτρια ομάδα.")]
    public long WinnerTeamId { get; set; }

    [DisplayName("Τύποι Παιχνιδιών (Προαιρετικό)")]
    public List<long>? SelectedGameTypeIds { get; set; }

    public IEnumerable<SelectListItem> HostTeamList { get; set; } = new List<SelectListItem>();
    
    public IEnumerable<SelectListItem> WinnerTeamList { get; set; } = new List<SelectListItem>();
    
    public IEnumerable<SelectListItem> GameTypeList { get; set; } = new List<SelectListItem>();
}