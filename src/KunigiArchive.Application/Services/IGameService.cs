using KunigiArchive.Application.Common;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KunigiArchive.Application.Services;

public interface IGameService
{
    Task<PaginatedResponse<MasterGameDetailsResponse>> GetPaginatedMasterGamesAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string? searchTerm = null);
    
    Task<ServiceResult> CreateMasterGameAsync(MasterGameCreateRequest request, ModelStateDictionary modelState);
}