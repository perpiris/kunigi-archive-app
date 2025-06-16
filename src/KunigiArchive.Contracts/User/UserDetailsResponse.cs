namespace KunigiArchive.Contracts.User;

public class UserDetailsResponse
{
    public required long ApplicationUserId { get; set; }
    
    public required string Email { get; set; }

    public IEnumerable<string>? Roles { get; set; } = [];
}