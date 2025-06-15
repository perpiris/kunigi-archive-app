using KunigiArchive.Application.Common;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.User;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KunigiArchive.Application.Services;

public interface IAccountService
{
    Task<PaginatedResponse<UserDetailsResponse>> GetPaginatedUsersAsync(
        int page,
        int pageSize,
        string sortBy = "email",
        bool ascending = true);
    
    Task<ServiceResult> CreateUserAsync(UserCreateRequest request, ModelStateDictionary modelState);

    Task<ServiceResult> ChangeEmailAsync(long userId, string newEmail, ModelStateDictionary modelState);

    Task<ServiceResult> UpdatePasswordAsync(long userId, string oldPassword, string newPassword,
        ModelStateDictionary modelState);
}