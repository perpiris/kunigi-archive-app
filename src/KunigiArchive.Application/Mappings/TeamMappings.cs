using KunigiArchive.Contracts.Team;
using KunigiArchive.Domain.Entities;

namespace KunigiArchive.Application.Mappings;

public static class TeamMappings
{
    public static TeamDetailsResponse MapToDetailsResponse(this Team team, bool includeFullDetails = false)
    {
        var response = new TeamDetailsResponse
        {
            TeamId = team.TeamId,
            Name = team.Name,
            Slug = team.Slug,
            IsActive = team.IsActive,
            IsArchived =  team.IsArchived,
        };

        if (includeFullDetails)
        {
            
        }

        return response;
    }
}