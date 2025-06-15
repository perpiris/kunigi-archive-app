using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.User;

namespace KunigiArchive.Application.Services;

public interface IAccountService
{
    Task<PaginatedResponse<UserDetailsResponse>> GetPaginatedUsersAsync(
        int page,
        int pageSize,
        string sortBy = "email",
        bool ascending = true);
}