namespace KunigiArchive.Contracts.User;

public class UserCreateRequest
{
    public required string Email { get; set; }

    public required string Role { get; set; }

    public long? TeamId { get; set; }
}