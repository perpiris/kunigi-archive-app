using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Game;
using KunigiArchive.Web.ViewModels.Common;
using KunigiArchive.Web.ViewModels.Game;

namespace KunigiArchive.Web.Mappings;

public static class GameMappings
{
    public static MasterGameDetailsViewModel MapToDetailsViewModel(this MasterGameDetailsResponse response)
    {
        return new MasterGameDetailsViewModel
        {
            MasterGameId = response.MasterGameId,
            Year = response.Year,
            Order = response.Order,
            Title = response.Title,
            SubTitle = response.SubTitle,
            Description = response.Description,
            HostTeamId = response.HostTeamId,
            HostTeamName = response.HostTeamName,
            WinnerTeamId = response.WinnerTeamId,
            WinnerTeamName = response.WinnerTeamName,
            LogoLink = response.LogoLink,
            IsArchived = response.IsArchived
        };
    }
    
    public static PaginatedViewModel<MasterGameDetailsViewModel> MapToPaginatedViewModel(this PaginatedResponse<MasterGameDetailsResponse> paginatedResponse)
    {
        return new PaginatedViewModel<MasterGameDetailsViewModel>
        {
            Items = paginatedResponse.Items.Select(x => x.MapToDetailsViewModel()).ToList(),
            CurrentPage = paginatedResponse.CurrentPage,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages
        };
    }

    public static MasterGameCreateRequest MapToCreateRequest(this MasterGameCreateViewModel viewModel)
    {
        return new MasterGameCreateRequest
        {
            Title = viewModel.Title,
            Year = viewModel.Year,
            Order = viewModel.Order,
            HostTeamId = viewModel.HostTeamId,
            WinnerTeamId = viewModel.WinnerTeamId,
            GameTypeIds = viewModel.SelectedGameTypeIds
        };
    }
}