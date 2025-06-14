namespace KunigiArchive.Web.ViewModels.Team;

public class TeamDetailsViewModel
{
    public long TeamId { get; set; }
    
    public required string Name { get; set; }
    
    public required string Slug { get; set; }
    
    public bool IsArchived { get; set; }
}