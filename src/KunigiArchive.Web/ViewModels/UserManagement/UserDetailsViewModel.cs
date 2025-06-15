namespace KunigiArchive.Web.ViewModels.UserManagement;

public class UserDetailsViewModel
{
    public long ApplicationUserId { get; set; }
    
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public IEnumerable<string> Roles { get; set; }
}