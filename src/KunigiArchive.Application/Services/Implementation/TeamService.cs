using KunigiArchive.Application.Data;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Domain.Entities;
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
    
    private static IQueryable<Team> ApplySorting(IQueryable<Team> query, string sortBy, bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "name" => ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name),
            _ => ascending ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
        };
    }
}