using KunigiArchive.Application.Data;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;
using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class GameService : IGameService
{
    private readonly DataContext _context;
    private readonly ILogger<GameService> _logger;

    public GameService(
        DataContext context, 
        ILogger<GameService> logger)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        _context = context;
        _logger = logger;
    }

    public async Task<PaginatedResponse<MasterGameDetailsResponse>> GetPaginatedMasterGamesAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string sortBy = "year",
        bool ascending = false,
        string? searchTerm = null)
    {
        var query = _context.MasterGames
            .Include(x => x.HostTeam)
            .Include(x => x.WinnerTeam)
            .AsNoTracking();

        if (!includeArchived)
        {
            query = query.Where(x => !x.IsArchived);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(x => 
                x.Title.Contains(searchTerm) ||
                (x.SubTitle != null && x.SubTitle.Contains(searchTerm)) ||
                x.Year.ToString().Contains(searchTerm));
        }

        query = ApplySorting(query, sortBy, ascending);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PaginatedResponse<MasterGameDetailsResponse>
        {
            Items = items.Select(x => x.MapToMasterGameDetailsResponse()).ToList(),
            CurrentPage = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };
    }

    private static IQueryable<MasterGame> ApplySorting(IQueryable<MasterGame> query, string sortBy, bool ascending)
    {
        return sortBy.ToLower() switch
        {
            "year" => ascending ? query.OrderBy(x => x.Year) : query.OrderByDescending(x => x.Year),
            "order" => ascending ? query.OrderBy(x => x.Order) : query.OrderByDescending(x => x.Order),
            "title" => ascending ? query.OrderBy(x => x.Title) : query.OrderByDescending(x => x.Title),
            _ => ascending ? query.OrderBy(x => x.Year) : query.OrderByDescending(x => x.Year)
        };
    }
}