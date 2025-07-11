﻿using KunigiArchive.Contracts.Common;
using KunigiArchive.Contracts.Team;
using KunigiArchive.Web.ViewModels.Common;
using KunigiArchive.Web.ViewModels.Team;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            YearFounded = response.YearFounded,
            Description =  response.Description,
            FacebookLink =  response.FacebookLink,
            InstagramLink =  response.InstagramLink,
            YoutubeLink =  response.YoutubeLink,
            WebsiteLink =  response.WebsiteLink
        };
    }
    
    public static PaginatedViewModel<TeamDetailsViewModel> MapToPaginatedTeamDetailsViewModel(this PaginatedResponse<TeamDetailsResponse> paginatedResponse)
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
        var userList = response.AvailableUsers
            .Select(x => new SelectListItem
            {
                Value = x.ApplicationUserId.ToString(),
                Text = x.Email
            })
            .ToList();

        return new TeamManagerDetailsViewModel
        {
            TeamId = response.TeamId,
            TeamName = response.TeamName,
            Slug = response.Slug,
            CurrentManagers = response.CurrentManagers
                .Select(x => x.MapToDetailsViewModel())
                .ToList(),
            AvailableUsers = new SelectList(userList, "Value", "Text")
        };
    }

    public static TeamMediaDetailsViewModel MapToTeamMediaDetailsViewModel(this TeamMediaDetailsResponse response)
    {
        return new TeamMediaDetailsViewModel
        {
            TeamId = response.TeamId,
            Slug =  response.Slug,
            TeamName =  response.TeamName,
            MediaFiles = response.MediaFiles.Select(x => x.MapToMediaFileViewModel()).ToList()
        };
    }
}