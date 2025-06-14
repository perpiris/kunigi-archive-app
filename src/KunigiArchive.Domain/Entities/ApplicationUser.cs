using Microsoft.AspNetCore.Identity;

namespace KunigiArchive.Domain.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public ICollection<TeamManager> ManagedTeams { get; set; } = new List<TeamManager>();
}