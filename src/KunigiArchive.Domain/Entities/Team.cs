namespace KunigiArchive.Domain.Entities;

public class Team
{
    public long TeamId { get; set; }

    public required string Name { get; set; }
    
    public required string Slug { get; set; }

    public bool IsActive { get; set; }

    public bool IsArchived { get; set; } = true;
    
    public short? YearFounded { get; set; }

    public string? Description { get; set; }
    
    public string? FacebookLink { get; set; }
    
    public string? InstagramLink { get; set; }
    
    public string? YoutubeLink { get; set; }
    
    public string? WebsiteLink { get; set; }
    
    public string? LogoLink { get; set; }
    
    public ICollection<TeamManager> Managers { get; set; } = new List<TeamManager>();
    
    public ICollection<TeamMediaFile> MediaFiles { get; set; } = new List<TeamMediaFile>();
}