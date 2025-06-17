namespace KunigiArchive.Contracts.Game;

public class MasterGameCreateRequest
{
    public required string Title { get; set; }
    
    public int? Year { get; set; }
    
    public int? Order { get; set; }
    
    public required long HostTeamId { get; set; }
    
    public required long WinnerTeamId { get; set; }
    
    public List<long>? GameTypeIds { get; set; }
}