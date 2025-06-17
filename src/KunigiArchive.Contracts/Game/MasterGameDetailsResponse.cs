namespace KunigiArchive.Contracts.Game;

public class MasterGameDetailsResponse
{
    public required long MasterGameId { get; set; }
    
    public required int Year { get; set; }
    
    public required int Order { get; set; }
    
    public required string Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Description { get; set; }
    
    public bool IsArchived { get; set; } = true;
    
    public long HostTeamId { get; set; }
    
    public required string HostTeamName { get; set; }
    
    public long WinnerTeamId { get; set; }
    
    public required string WinnerTeamName { get; set; }
    
    public string? LogoLink { get; set; }
}