using KunigiArchive.Contracts.Team;
using KunigiArchive.Contracts.User;
using KunigiArchive.Domain.Entities;

namespace KunigiArchive.Application.Mappings;

public static class TeamMappings
{
    public static TeamDetailsResponse MapToTeamDetailsResponse(this Team team, bool includeFullDetails = false)
    {
        var response = new TeamDetailsResponse
        {
            TeamId = team.TeamId,
            Name = team.Name,
            Slug = team.Slug,
            IsActive = team.IsActive,
            IsArchived =  team.IsArchived,
            YearFounded =  team.YearFounded,
            Description =  team.Description,
            FacebookLink =  team.FacebookLink,
            InstagramLink = team.InstagramLink,
            YoutubeLink =  team.YoutubeLink,
            WebsiteLink =   team.WebsiteLink,
            LogoLink =    team.LogoLink
        };

        if (includeFullDetails)
        {
            
        }

        return response;
    }

    public static UserDetailsResponse MapToUserDetailsResponse(this TeamManager teamManager)
    {
        return new UserDetailsResponse
        {
            ApplicationUserId =  teamManager.ApplicationUserId,
            Email = teamManager.ApplicationUser.Email!
        };
    }
}