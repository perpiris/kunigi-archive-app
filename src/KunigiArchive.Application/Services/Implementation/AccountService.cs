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
    private readonly ITeamService _teamService;

    public AccountService(
        UserManager<ApplicationUser> userManager,
        ILogger<AccountService> logger, 
        ITeamService teamService)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(teamService);

        _userManager = userManager;
        _logger = logger;
        _teamService = teamService;
    }
    
    public async Task<PaginatedResponse<UserDetailsResponse>> GetPaginatedUsersAsync(
        int page,
        int pageSize,
        string? searchTerm = null)
    {
        var query = _userManager.Users.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => x.Email!.Contains(searchTerm));
        }

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
            _logger.LogWarning("User creation failed for email {Email}. Errors: {Errors}", request.Email, string.Join(", ", identityResult.Errors.Select(e => e.Description)));
            foreach (var error in identityResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, request.Role);
        if (!roleResult.Succeeded)
        {
            _logger.LogWarning("Failed to add role {Role} to user {Email}. Errors: {Errors}", request.Role, user.Email, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
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

            var addManagerResult = await _teamService.AddTeamManagerAsync(request.TeamId.Value.ToString(), user.Id);
            if (!addManagerResult.IsSuccess)
            {
                _logger.LogWarning("Failed to add user {Email} as manager for TeamId {TeamId}. Error: {Error}", user.Email, request.TeamId, addManagerResult.Message);
                modelState.AddModelError(string.Empty, addManagerResult.Message);
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
            _logger.LogWarning("Attempted to change email for a non-existent user with ID {UserId}", userId);
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

        var originalEmail = user.Email;
        var setEmailResult = await _userManager.SetEmailAsync(user, newEmail);
        if (!setEmailResult.Succeeded)
        {
            _logger.LogWarning("Failed to set email for user {UserId}. Errors: {Errors}", userId, string.Join(", ", setEmailResult.Errors.Select(e => e.Description)));
            foreach (var error in setEmailResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }
        
        var setUserNameResult = await _userManager.SetUserNameAsync(user, newEmail);
        if (!setUserNameResult.Succeeded)
        {
            _logger.LogError("Failed to set username for user {UserId} after email was already changed. Attempting to revert email change. Errors: {Errors}", userId, string.Join(", ", setUserNameResult.Errors.Select(e => e.Description)));
            await _userManager.SetEmailAsync(user, originalEmail); 
            
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
        if (string.IsNullOrEmpty(newPassword))
        {
            modelState.AddModelError("NewPassword", "New password is required.");
        }
        
        if (string.IsNullOrEmpty(oldPassword))
        {
            modelState.AddModelError("OldPassword", "Current password is required to set a new password.");
        }

        if (!modelState.IsValid)
        {
            return ServiceResult.Failure();
        }

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null)
        {
            _logger.LogWarning("Attempted to update password for a non-existent user with ID {UserId}", userId);
            modelState.AddModelError(string.Empty, "User not found.");
            return ServiceResult.Failure();
        }

        var changePasswordResult = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
        if (!changePasswordResult.Succeeded)
        {
            _logger.LogWarning("Password change failed for user {UserId}. Errors: {Errors}", userId, string.Join(", ", changePasswordResult.Errors.Select(e => e.Description)));
            foreach (var error in changePasswordResult.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
            return ServiceResult.Failure();
        }

        return ServiceResult.Success();
    }
}