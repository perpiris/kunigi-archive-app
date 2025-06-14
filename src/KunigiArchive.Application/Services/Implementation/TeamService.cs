using KunigiArchive.Application.Common;
using KunigiArchive.Application.Data;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Application.Utilities;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class TeamService : ITeamService
{
    private readonly DataContext _context;
    private readonly ILogger<TeamService> _logger;

    public TeamService(DataContext context, ILogger<TeamService> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
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
            Items = items.Select(x => x.MapToDetailsResponse()).ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
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

        return team?.MapToDetailsResponse(includeFullDetails);
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