using KunigiArchive.Application.Common;
using KunigiArchive.Application.Data;
using KunigiArchive.Application.Mappings;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;
using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KunigiArchive.Application.Services.Implementation;

public class GameService : IGameService
{
    private readonly DataContext _context;
    private readonly ILogger<GameService> _logger;
    private readonly IFileService _fileService;

    public GameService(
        DataContext context, 
        ILogger<GameService> logger, 
        IFileService fileService)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(fileService);

        _context = context;
        _logger = logger;
        _fileService = fileService;
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
    
    public async Task<ServiceResult> CreateMasterGameAsync(MasterGameCreateRequest request, ModelStateDictionary modelState)
    {
        if (!request.Year.HasValue)
        {
            modelState.AddModelError("Year", "Το πεδίο απαιτείται.");
        }
        
        if (!request.Order.HasValue)
        {
            modelState.AddModelError("Order", "Το πεδίο απαιτείται.");
        }
        
        var yearExists = await _context.MasterGames.AnyAsync(x => x.Year == request.Year);
        if (yearExists)
        {
            modelState.AddModelError("Year", "Υπάρχει ήδη παιχνίδι για αυτό το έτος.");
        }
        
        var orderExists = await _context.MasterGames.AnyAsync(x => x.Order == request.Order);
        if (orderExists)
        {
            modelState.AddModelError("Order", "Υπάρχει ήδη παιχνίδι με αυτή τη σειρά.");
        }
        
        var hostTeamExists = await _context.Teams.AnyAsync(x => x.TeamId == request.HostTeamId);
        if (!hostTeamExists)
        {
            modelState.AddModelError("HostTeamId", "Η ομάδα διοργανώτρια δεν βρέθηκε.");
        }
        
        var winnerTeamExists = await _context.Teams.AnyAsync(x => x.TeamId == request.WinnerTeamId);
        if (!winnerTeamExists)
        {
            modelState.AddModelError("WinnerTeamId", "Η ομάδα νικήτρια δεν βρέθηκε.");
        }

        if (!modelState.IsValid)
        {
            return ServiceResult.Failure();
        }
        
        await _fileService.CreateMediaFolderAsync($"games/{request.Year}");

        var newMasterGame = new MasterGame
        {
            Title = request.Title.Trim(),
            SubTitle = $"{request.Order}ο Κυνήθι Θησαυρού",
            Year = request.Year!.Value,
            Order = request.Order!.Value,
            HostTeamId = request.HostTeamId,
            WinnerTeamId = request.WinnerTeamId
        };
        
        _context.MasterGames.Add(newMasterGame);
        await _context.SaveChangesAsync();
        
        return ServiceResult.Success();
    }
    
    public async Task<(IEnumerable<SelectListItem> HostTeams, IEnumerable<SelectListItem> WinnerTeams, IEnumerable<SelectListItem> GameTypes)> GetCreateMasterGameSelectListsAsync()
    {
        var teams = await _context.Teams.OrderBy(x => x.Name).ToListAsync();
        var gameTypes = await _context.GameTypes.OrderBy(x => x.Label).ToListAsync();

        var teamSelectList = teams.Select(team => new SelectListItem
        {
            Value = team.TeamId.ToString(),
            Text = team.Name
        });

        var gameTypeSelectList = gameTypes.Select(gameType => new SelectListItem
        {
            Value = gameType.GameTypeId.ToString(),
            Text = gameType.Label
        });

        var selectListItems = teamSelectList.ToList();
        return (selectListItems, selectListItems, gameTypeSelectList);
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