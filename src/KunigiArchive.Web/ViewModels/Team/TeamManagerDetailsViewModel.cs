using KunigiArchive.Web.ViewModels.UserManagement;

namespace KunigiArchive.Web.ViewModels.Team;

public class TeamManagerDetailsViewModel
{
    public long TeamId { get; set; }
    
    public required string TeamName { get; set; }
    
    public required string Slug { get; set; }
    
    public List<UserDetailsViewModel> CurrentManagers { get; set; } = new();
    
    public List<UserDetailsViewModel> AvailableUsers { get; set; } = new();
}