using KunigiArchive.Contracts.Game;
using KunigiArchive.Domain.Entities;

namespace KunigiArchive.Application.Mappings;

public static class GameMappings
{
    public static MasterGameDetailsResponse MapToMasterGameDetailsResponse(this MasterGame masterGame)
    {
        return new MasterGameDetailsResponse
        {
            MasterGameId = masterGame.MasterGameId,
            Year = masterGame.Year,
            Order = masterGame.Order,
            OrderTitle = masterGame.OrderTitle,
            Title = masterGame.Title,
            Description = masterGame.Description,
            HostTeamId = masterGame.HostTeamId,
            HostTeamName = masterGame.HostTeam.Name,
            WinnerTeamId = masterGame.WinnerTeamId,
            WinnerTeamName = masterGame.WinnerTeam.Name,
            LogoLink = masterGame.LogoLink,
            IsArchived = masterGame.IsArchived
        };
    }
}