using KunigiArchive.Application.Mappings;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity;
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
    
    private static IQueryable<ApplicationUser> ApplySorting(IQueryable<ApplicationUser> query, string sortBy, bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "email" => ascending ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email),
            _ => ascending ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email)
        };
    }
}