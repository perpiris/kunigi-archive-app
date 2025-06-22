namespace KunigiArchive.Contracts.Game;

public class MasterGameCreateRequest
{
    public required int Year { get; set; }
    
    public required int Order { get; set; }
    
    public required long HostTeamId { get; set; }
    
    public required long WinnerTeamId { get; set; }
}