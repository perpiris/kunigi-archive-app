using KunigiArchive.Application.Common;
using KunigiArchive.Application.Data;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Application.Utilities;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class TeamService : ITeamService
{
    private readonly DataContext _context;
    private readonly ILogger<TeamService> _logger;
    private readonly IFileService _fileService;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeamService(
        DataContext context, 
        ILogger<TeamService> logger, 
        IFileService fileService, 
        UserManager<ApplicationUser> userManager)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(fileService);
        ArgumentNullException.ThrowIfNull(userManager);

        _context = context;
        _logger = logger;
        _fileService = fileService;
        _userManager = userManager;
    }

    public async Task<PaginatedResponse<TeamDetailsResponse>> GetPaginatedTeamsAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string sortBy,
        bool ascending)
    {
        var query = _context.Teams.AsNoTracking();

        if (!includeArchived)
        {
            query = query.Where(x => !x.IsArchived);
        }

        query = ApplySorting(query, sortBy, ascending);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<TeamDetailsResponse>
        {
            Items = items.Select(x => x.MapToTeamDetailsResponse()).ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    public async Task<IEnumerable<TeamDetailsResponse>> GetAllTeamsAsync()
    {
        var items = await _context.Teams.ToListAsync();
        return items.Select(x => x.MapToTeamDetailsResponse()).ToList();
    }

    public async Task<ServiceResult> CreateTeamAsync(TeamCreateRequest request, ModelStateDictionary modelState)
    {
        var slug = SlugGenerator.GenerateSlug(request.Name);

        var nameExists = await _context.Teams.AnyAsync(x => x.Slug == slug);
        if (nameExists)
        {
            modelState.AddModelError("Name", "Υπάρχει ήδη ομάδα με αυτό το όνομα.");
        }

        if (!modelState.IsValid)
        {
            return ServiceResult.Failure();
        }
        
        await _fileService.CreateMediaFolderAsync($"teams/{slug}");

        var newTeam = new Team
        {
            Name = request.Name.Trim(),
            Slug = slug,
            IsActive = request.IsActive,
            IsArchived = true
        };
        
        _context.Teams.Add(newTeam);
        await _context.SaveChangesAsync();
        
        return ServiceResult.Success();
    }

    public async Task<ServiceResult> EditTeamAsync(TeamEditRequest request, IFormFile? image, ModelStateDictionary modelState)
    {
        var team = await _context.Teams.FirstOrDefaultAsync(x => x.Slug == request.Slug);

        if (team is null)
        {
            return ServiceResult.Failure("Η ομάδα δεν βρέθηκε");
        }
    
        team.IsActive = request.IsActive;
        team.IsArchived = request.IsArchived;
        team.YearFounded = request.YearFounded;
        team.Description = request.Description;
        team.FacebookLink = request.FacebookLink;
        team.InstagramLink = request.InstagramLink;
        team.YoutubeLink = request.YoutubeLink;
        team.WebsiteLink = request.WebsiteLink;

        if (image is not null)
        {
            _fileService.DeleteFile(team.LogoLink);
        
            var saveResult = await _fileService.SaveFileAsync(image, $"teams/{team.Slug}");
            if (saveResult.IsSuccess)
            {
                team.LogoLink = saveResult.Value;
            }
            else
            {
                _logger.LogWarning("Could not save new logo for team {TeamSlug}: {ErrorMessage}", team.Slug, saveResult.Message);
            }
        }
        
        if (!modelState.IsValid)
        {
            return ServiceResult.Failure();
        }
    
        _context.Teams.Update(team);
        await _context.SaveChangesAsync();
    
        return ServiceResult.Success();
    }

    public async Task<bool> CanUserAccessTeam(long userId, string idOrSlug)
    {
        Team? team;
        if (long.TryParse(idOrSlug, out var teamId))
        {
            team = await _context.Teams
                .FirstOrDefaultAsync(x => x.TeamId == teamId);
        }
        else
        {
            team = await _context.Teams
                .FirstOrDefaultAsync(x => x.Slug == idOrSlug);
        }

        if (team == null)
        {
            return false;
        }

        return await _context.TeamManagers
            .AnyAsync(x => x.TeamId == team.TeamId && x.ApplicationUserId == userId);
    }
    
    public async Task<TeamDetailsResponse?> GetTeamByIdOrSlugAsync(string idOrSlug, bool includeFullDetails)
    {
        var query = _context.Teams.AsQueryable();
        if (includeFullDetails)
        {
            query = query
                .Include(x => x.Managers)
                .AsSplitQuery();
        }

        Team? team;
        if (long.TryParse(idOrSlug, out var id))
        {
            team = await query.FirstOrDefaultAsync(x => x.TeamId == id);
        }
        else
        {
            team = await query.FirstOrDefaultAsync(x => x.Slug == idOrSlug);
        }

        return team?.MapToTeamDetailsResponse(includeFullDetails);
    }

    public async Task<IEnumerable<TeamDetailsResponse>> GetManagerTeamsAsync(string userIdString)
    {
        var userId = long.Parse(userIdString);

        var teamList =
            await _context.TeamManagers
                .Include(x => x.Team)
                .Where(x => x.ApplicationUserId == userId)
                .Select(x => x.Team)
                .ToListAsync();

        return teamList
            .Select(x => x.MapToTeamDetailsResponse())
            .ToList();
    }
    
    public async Task<TeamManagerDetailsResponse?> GetTeamWithManagersAsync(string idOrSlug)
    {
        Team? team;
        if (long.TryParse(idOrSlug, out var teamId))
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamId == teamId);
        }
        else
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.Slug == idOrSlug);
        }

        if (team is null)
        {
            return null;
        }

        var teamManagers = await _context.TeamManagers
            .Include(x => x.ApplicationUser)
            .Where(x => x.TeamId == team.TeamId)
            .ToListAsync();

        var currentManagerIds = teamManagers.Select(x => x.ApplicationUserId).ToHashSet();

        var users = await _context.Users
            .ToListAsync();

        var currentManagers = teamManagers
            .Select(x => x.MapToUserDetailsResponse())
            .ToList();

        var availableUsers = users
            .Where(x => !currentManagerIds.Contains(x.Id))
            .Select(x => x.MapToUserDetailsResponse(null))
            .ToList();

        return new TeamManagerDetailsResponse
        {
            TeamId = team.TeamId,
            TeamName = team.Name,
            Slug = team.Slug,
            CurrentManagers = currentManagers,
            AvailableUsers = availableUsers
        };
    }

    public async Task<ServiceResult> AddTeamManagerAsync(string idOrSlug, long applicationUserId)
    {
        Team? team;
        if (long.TryParse(idOrSlug, out var teamId))
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamId == teamId);
        }
        else
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.Slug == idOrSlug);
        }

        if (team is null)
        {
            return ServiceResult.Failure("Η ομάδα δεν βρέθηκε");
        }

        var userExists = await _context.Users.AnyAsync(x => x.Id == applicationUserId);
        if (!userExists)
        {
            return ServiceResult.Failure("Ο χρήστης δεν βρέθηκε");
        }

        var managerExists = await _context.TeamManagers
            .AnyAsync(x => x.TeamId == team.TeamId && x.ApplicationUserId == applicationUserId);
        if (managerExists)
        {
            return ServiceResult.Failure("Ο χρήστης είναι ήδη διαχειριστής της ομάδας");
        }

        var teamManager = new TeamManager
        {
            TeamId = team.TeamId,
            ApplicationUserId = applicationUserId
        };

        _context.TeamManagers.Add(teamManager);
        await _context.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
        if (user is not null)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, "Manager");
            if (!isInRole)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "Manager");
                if (!roleResult.Succeeded)
                {
                    _logger.LogWarning("Failed to add Manager role to user {UserId} when adding to team {TeamId}. Errors: {Errors}", applicationUserId, team.TeamId, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        return ServiceResult.Success();
    }

    public async Task<ServiceResult> RemoveTeamManagerAsync(string idOrSlug, long applicationUserId)
    {
        Team? team;
        if (long.TryParse(idOrSlug, out var teamId))
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.TeamId == teamId);
        }
        else
        {
            team = await _context.Teams.FirstOrDefaultAsync(x => x.Slug == idOrSlug);
        }

        if (team is null)
        {
            return ServiceResult.Failure("Η ομάδα δεν βρέθηκε");
        }

        var teamManager = await _context.TeamManagers
            .FirstOrDefaultAsync(x => x.TeamId == team.TeamId && x.ApplicationUserId == applicationUserId);

        if (teamManager is null)
        {
            return ServiceResult.Failure("Ο διαχειριστής δεν βρέθηκε");
        }

        _context.TeamManagers.Remove(teamManager);
        await _context.SaveChangesAsync();

        var user = await _userManager.FindByIdAsync(applicationUserId.ToString());
        if (user is not null)
        {
            var isInRole = await _userManager.IsInRoleAsync(user, "Manager");
            if (isInRole)
            {
                var hasOtherTeams = await _context.TeamManagers
                    .AnyAsync(x => x.ApplicationUserId == applicationUserId);
                
                if (!hasOtherTeams)
                {
                    var roleResult = await _userManager.RemoveFromRoleAsync(user, "Manager");
                    if (!roleResult.Succeeded)
                    {
                        _logger.LogWarning("Failed to remove Manager role from user {UserId} when removing from team {TeamId}. Errors: {Errors}", applicationUserId, team.TeamId, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
        }

        return ServiceResult.Success();
    }
    
    private static IQueryable<Team> ApplySorting(IQueryable<Team> query, string sortBy, bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
            _ => ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };
    }
}