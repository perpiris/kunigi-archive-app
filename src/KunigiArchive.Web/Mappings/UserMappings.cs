﻿using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.User;
using KunigiArchive.Web.ViewModels.Common;
using KunigiArchive.Web.ViewModels.UserManagement;

namespace KunigiArchive.Web.Mappings;

public static class UserMappings
{
    public static UserDetailsViewModel MapToDetailsViewModel(this UserDetailsResponse response)
    {
        return new UserDetailsViewModel
        {
            ApplicationUserId = response.ApplicationUserId,
            Email = response.Email,
            Roles = response.Roles,
        };
    }
    
    public static PaginatedViewModel<UserDetailsViewModel> MapToPaginatedViewModel(this PaginatedResponse<UserDetailsResponse> paginatedResponse)
    {
        return new PaginatedViewModel<UserDetailsViewModel>
        {
            Items = paginatedResponse.Items.Select(x => x.MapToDetailsViewModel()).ToList(),
            CurrentPage = paginatedResponse.CurrentPage,
            PageSize = paginatedResponse.PageSize,
            TotalPages = paginatedResponse.TotalPages
        };
    }

    public static UserCreateRequest MapToCreateUserRequest(this UserCreateViewModel viewModel)
    {
        return new UserCreateRequest
        {
            Email = viewModel.Email,
            Role = viewModel.SelectedRole,
            TeamId = viewModel.SelectedTeamId
        };
    }
}