namespace KunigiArchive.Web.ViewModels.Team;

public class TeamDetailsViewModel
{
    public long TeamId { get; set; }
    
    public required string Name { get; set; }
    
    public required string Slug { get; set; }
    
    public bool IsActive { get; set; }
    
    public bool IsArchived { get; set; }
    
    public short? YearFounded { get; set; }

    public string? Description { get; set; } = string.Empty;
    
    public string? FacebookLink { get; set; } = string.Empty;
    
    public string? InstagramLink { get; set; } = string.Empty;
    
    public string? YoutubeLink { get; set; } = string.Empty;
    
    public string? WebsiteLink { get; set; } = string.Empty;
    
    public string? LogoLink { get; set; } = string.Empty;
}