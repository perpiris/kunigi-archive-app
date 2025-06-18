namespace KunigiArchive.Domain.Entities;

public class MasterGame
{
    public long MasterGameId { get; set; }

    public required int Year { get; set; }

    public required int Order { get; set; }
    
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsArchived { get; set; } = true;

    public long HostTeamId { get; set; }
    
    public long WinnerTeamId { get; set; }
    
    public string? LogoLink { get; set; }

    public Team HostTeam { get; set; } = null!;
    
    public Team WinnerTeam { get; set; } = null!;

    public ICollection<Game> Games { get; set; } = new List<Game>();
    
    public ICollection<MasterGameMediaFile> MediaFiles { get; set; } = new List<MasterGameMediaFile>();
}