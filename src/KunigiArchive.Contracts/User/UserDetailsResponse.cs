namespace KunigiArchive.Contracts.User;

public record UserDetailsResponse(
    long ApplicationUserId, 
    string UserName, 
    string Email, 
    IEnumerable<string> Roles);