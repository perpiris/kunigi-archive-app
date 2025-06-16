using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Web.ViewModels.Common;
using KunigiArchive.Web.ViewModels.Team;

namespace KunigiArchive.Web.Mappings;

public static class TeamMappings
{
    public static TeamDetailsViewModel MapToDetailsViewModel(this TeamDetailsResponse response)
    {
        return new TeamDetailsViewModel
        {
            TeamId = response.TeamId,
            Name = response.Name,
            Slug = response.Slug,
            IsActive = response.IsActive,
            IsArchived = response.IsArchived,
            YearFounded = response.YearFounded,
            Description = response.Description,
            FacebookLink = response.FacebookLink,
            InstagramLink = response.InstagramLink,
            YoutubeLink = response.YoutubeLink,
            WebsiteLink = response.WebsiteLink,
            LogoLink =  response.LogoLink
        };
    }

    public static TeamEditViewModel MapToEditViewModel(this TeamDetailsResponse response)
    {
        return new TeamEditViewModel
        {
            TeamId = response.TeamId,
            Name = response.Name,
            Slug = response.Slug,
            IsActive = response.IsActive,
            IsArchived = response.IsArchived,
        };
    }
    
    public static PaginatedViewModel<TeamDetailsViewModel> MapToPaginatedViewModel(this PaginatedResponse<TeamDetailsResponse> paginatedResponse)
    {
        return new PaginatedViewModel<TeamDetailsViewModel>
        {
            Items = paginatedResponse.Items.Select(x => x.MapToDetailsViewModel()).ToList(),
            CurrentPage = paginatedResponse.CurrentPage,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages
        };
    }

    public static TeamCreateRequest MapToCreateRequest(this TeamCreateViewModel viewModel)
    {
        return new TeamCreateRequest(
            viewModel.Name, 
            viewModel.IsActive);
    }
    
    public static TeamEditRequest MapToEditRequest(this TeamEditViewModel viewModel)
    {
        return new TeamEditRequest(
            viewModel.Slug, 
            viewModel.IsActive, 
            viewModel.IsArchived, 
            viewModel.YearFounded, 
            viewModel.Description, 
            viewModel.FacebookLink, 
            viewModel.InstagramLink, 
            viewModel.YoutubeLink,  
            viewModel.WebsiteLink);
    }

    public static TeamManagerDetailsViewModel MapToTeamManagerDetailsViewModel(this TeamManagerDetailsResponse response)
    {
        return new TeamManagerDetailsViewModel
        {
            TeamId = response.TeamId,
            TeamName =  response.TeamName,
            Slug =   response.Slug,
            CurrentManagers =   response.CurrentManagers.Select(x => x.MapToDetailsViewModel()).ToList(),
            AvailableUsers =  response.AvailableUsers.Select(x => x.MapToDetailsViewModel()).ToList()
        };
    }
}