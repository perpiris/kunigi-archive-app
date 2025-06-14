using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Web.ViewModels.Common;
using KunigiArchive.Web.ViewModels.Team;

namespace KunigiArchive.Web.Mappings;

public static class TeamMappings
{
    public static TeamDetailsViewModel ToDetailsViewModel(this TeamDetailsResponse response)
    {
        return new TeamDetailsViewModel
        {
            TeamId = response.TeamId,
            Name = response.Name,
            Slug = response.Slug,
            IsArchived = response.IsArchived,
        };
    }
    
    public static PaginatedViewModel<TeamDetailsViewModel> ToPaginatedViewModel(this PaginatedResponse<TeamDetailsResponse> paginatedResponse)
    {
        return new PaginatedViewModel<TeamDetailsViewModel>
        {
            Items = paginatedResponse.Items.Select(x => x.ToDetailsViewModel()).ToList(),
            CurrentPage = paginatedResponse.CurrentPage,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages
        };
    }
}