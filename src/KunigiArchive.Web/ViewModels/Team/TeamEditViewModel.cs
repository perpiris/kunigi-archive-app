using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KunigiArchive.Web.ViewModels.Team;

public class TeamEditViewModel
{
    public required long TeamId { get; set; }
    
    public required string Name { get; set; }
    
    public required string Slug { get; set; }
    
    [DisplayName("Ενεργή")]
    public bool IsActive { get; set; }
    
    [DisplayName("Αποκρυμένη")]
    public bool IsArchived { get; set; }
    
    [DisplayName("Έτος Δημιουργίας")]
    public short? YearFounded { get; set; }
    
    [DisplayName("Περιγραφή")]
    [StringLength(4000)]
    public string? Description { get; set; }
    
    [DisplayName("Facebook")]
    [StringLength(500)]
    public string? FacebookLink { get; set; }
    
    [DisplayName("Instagram")]
    [StringLength(500)]
    public string? InstagramLink { get; set; }
    
    [DisplayName("Youtube")]
    [StringLength(500)]
    public string? YoutubeLink { get; set; }
    
    [DisplayName("Website")]
    [StringLength(500)]
    public string? WebsiteLink { get; set; }

    [DisplayName("Φωτογραφία")] 
    public string? Image { get; set; }
}