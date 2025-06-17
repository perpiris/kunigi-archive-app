using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;

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
}