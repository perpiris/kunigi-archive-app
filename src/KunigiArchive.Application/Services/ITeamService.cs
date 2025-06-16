using KunigiArchive.Application.Common;
using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace KunigiArchive.Application.Services;

public interface ITeamService
{
    Task<PaginatedResponse<TeamDetailsResponse>> GetPaginatedTeamsAsync(
        int page,
        int pageSize,
        bool includeArchived,
        string? searchTerm = null);
    
    Task<IEnumerable<TeamDetailsResponse>> GetAllTeamsAsync();

    Task<ServiceResult> CreateTeamAsync(TeamCreateRequest request, ModelStateDictionary modelState);
    
    Task<ServiceResult> EditTeamAsync(TeamEditRequest request, IFormFile? image,
        ModelStateDictionary modelState);
    
    Task<bool> CanUserAccessTeam(long userId, string idOrSlug);
    
    Task<TeamDetailsResponse?> GetTeamByIdOrSlugAsync(string idOrSlug, bool includeFullDetails);

    Task<IEnumerable<TeamDetailsResponse>> GetManagerTeamsAsync(string userIdString);

    Task<TeamManagerDetailsResponse?> GetTeamWithManagersAsync(string idOrSlug);

    Task<ServiceResult> AddTeamManagerAsync(string idOrSlug, long applicationUserId);

    Task<ServiceResult> RemoveTeamManagerAsync(string idOrSlug, long applicationUserId);
}