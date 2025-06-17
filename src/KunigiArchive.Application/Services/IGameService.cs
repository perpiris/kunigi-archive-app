using KunigiArchive.Application.Common;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KunigiArchive.Application.Services;

public interface IGameService
{
    Task<PaginatedResponse<MasterGameDetailsResponse>> GetPaginatedMasterGamesAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string sortBy = "year",
        bool ascending = false,
        string? searchTerm = null);
    
    Task<ServiceResult> CreateMasterGameAsync(MasterGameCreateRequest request, ModelStateDictionary modelState);
    
    Task<(IEnumerable<SelectListItem> HostTeams, IEnumerable<SelectListItem> WinnerTeams, IEnumerable<SelectListItem> GameTypes)> GetCreateMasterGameSelectListsAsync();
}