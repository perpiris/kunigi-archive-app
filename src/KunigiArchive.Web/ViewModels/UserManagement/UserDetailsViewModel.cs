namespace KunigiArchive.Web.ViewModels.UserManagement;

public class UserDetailsViewModel
{
    public long ApplicationUserId { get; set; }
    
    public required string Email { get; set; }
    
    public IEnumerable<string>? Roles { get; set; }
}