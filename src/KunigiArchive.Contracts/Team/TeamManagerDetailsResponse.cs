using KunigiArchive.Contracts.User;

namespace KunigiArchive.Contracts.Team;

public class TeamManagerDetailsResponse
{
    public long TeamId { get; set; }
    
    public required string TeamName { get; set; }
    
    public required string Slug { get; set; }
    
    public List<UserDetailsResponse> CurrentManagers { get; set; } = new();
    
    public List<UserDetailsResponse> AvailableUsers { get; set; } = new();
}