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
                (!string.IsNullOrWhiteSpace(x.Title) && x.Title.ToLower().Contains(searchTerm.ToLower())) ||
                x.OrderTitle.Contains(searchTerm) ||
                x.Year.ToString().Contains(searchTerm));
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .OrderBy(x => x.Year)
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
            OrderTitle = $"{request.Order}ο Κυνήγι Θησαυρού",
            Year = request.Year,
            Order = request.Order,
            HostTeamId = request.HostTeamId,
            WinnerTeamId = request.WinnerTeamId
        };
        
        _context.MasterGames.Add(newMasterGame);
        await _context.SaveChangesAsync();
        
        return ServiceResult.Success();
    }
}