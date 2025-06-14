using Microsoft.AspNetCore.Identity;

namespace KunigiArchive.Domain.Entities;

public class ApplicationUser : IdentityUser<long>
{
    public bool IsApproved { get; set; } = false;
    
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}