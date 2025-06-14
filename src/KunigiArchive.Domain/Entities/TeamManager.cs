namespace KunigiArchive.Domain.Entities;

public class TeamManager
{
    public long ApplicationUserId { get; set; }
    
    public long TeamId { get; set; }

    public ApplicationUser ApplicationUser { get; set; } = null!;
    
    public Team Team { get; set; } = null!;
}