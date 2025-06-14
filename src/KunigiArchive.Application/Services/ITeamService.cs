using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;

namespace KunigiArchive.Application.Services;

public interface ITeamService
{
    Task<PaginatedResponse<TeamDetailsResponse>> GetPaginatedTeamsAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string sortBy,
        bool ascending);
}