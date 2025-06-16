using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;

namespace KunigiArchive.Application.Mappings;

public static class UserMappings
{
    public static UserDetailsResponse MapToUserDetailsResponse(this ApplicationUser user, IEnumerable<string>? roles)
    {
        return new UserDetailsResponse
        {
            ApplicationUserId =  user.Id,
            Email = user.Email,
            Roles = roles,
        };
    }
}