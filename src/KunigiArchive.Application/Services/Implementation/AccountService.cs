using KunigiArchive.Application.Common;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class AccountService : IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ILogger<AccountService> _logger;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        ILogger<AccountService> logger)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(logger);

        _userManager = userManager;
        _logger = logger;
    }
    
    public async Task<PaginatedResponse<UserDetailsResponse>> GetPaginatedUsersAsync(
        int page,
        int pageSize,
        string sortBy = "email",
        bool ascending = true)
    {
        var query = _userManager.Users.AsNoTracking();

        query = ApplySorting(query, sortBy, ascending);

        var totalCount = await query.CountAsync();
        var users = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var userList = new List<UserDetailsResponse>();
        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userList.Add(user.MapToUserDetailsResponse(roles));
        }

        return new PaginatedResponse<UserDetailsResponse>
        {
            Items = userList,
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }

    public async Task<ServiceResult> CreateUserAsync(UserCreateRequest request, ModelStateDictionary modelState)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser is not null)
        {
            modelState.AddModelError(nameof(request.Email), "Υπάρχει ήδη ένας χρήστης με αυτό το email.");
            return ServiceResult.Failure();
        }
    
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email
        };

        var identityResult = await _userManager.CreateAsync(user, request.Email);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            await _userManager.DeleteAsync(user);
            return ServiceResult.Failure();
        }

        if (request.Role == "Manager")
        {
            if (request.TeamId is null)
            {
                modelState.AddModelError(nameof(request.TeamId), "TeamId must be provided for the Manager role.");
                await _userManager.DeleteAsync(user);
                return ServiceResult.Failure();
            }

            user.ManagedTeams.Add(new TeamManager
            {
                TeamId = request.TeamId.Value
            });

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    modelState.AddModelError(string.Empty, error.Description);
                }
                await _userManager.DeleteAsync(user);
                return ServiceResult.Failure();
            }
        }
    
        return ServiceResult.Success();
    }
    
    public async Task<ServiceResult> ChangeEmailAsync(long userId, string newEmail, ModelStateDictionary modelState)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            modelState.AddModelError(string.Empty, "User not found.");
            return ServiceResult.Failure();
        }

        if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
        {
            modelState.AddModelError(nameof(newEmail), "The new email must be different from the current email.");
            return ServiceResult.Failure();
        }

        var existingUser = await _userManager.FindByEmailAsync(newEmail);
        if (existingUser is not null)
        {
            modelState.AddModelError(nameof(newEmail), "A user with this email already exists.");
            return ServiceResult.Failure();
        }

        var setEmailResult = await _userManager.SetEmailAsync(user, newEmail);
        if (!setEmailResult.Succeeded)
        {
            foreach (var error in setEmailResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }
        
        var setUserNameResult = await _userManager.SetUserNameAsync(user, newEmail);
        if (!setUserNameResult.Succeeded)
        {
            await _userManager.SetEmailAsync(user, user.Email); 
            
            foreach (var error in setUserNameResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> UpdatePasswordAsync(long userId, string oldPassword, string newPassword, ModelStateDictionary modelState)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            modelState.AddModelError(string.Empty, "User not found.");
            return ServiceResult.Failure();
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!changePasswordResult.Succeeded)
        {
            foreach (var error in changePasswordResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }

        return ServiceResult.Success();
    }

    private static IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, string sortBy, bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "email" => ascending ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email),
            _ => ascending ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email)
        };
    }
}