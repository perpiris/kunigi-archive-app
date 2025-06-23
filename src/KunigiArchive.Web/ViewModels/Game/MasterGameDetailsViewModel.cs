namespace KunigiArchive.Web.ViewModels.Game;

public class MasterGameDetailsViewModel
{
    public long MasterGameId { get; set; }
    
    public required int Year { get; set; }
    
    public required int Order { get; set; }
    
    public required string OrderTitle { get; set; }
    
    public string? Title { get; set; }
    
    public string? Description { get; set; }
    
    public long HostTeamId { get; set; }
    
    public required string HostTeamName { get; set; }
    
    public long WinnerTeamId { get; set; }
    
    public required string WinnerTeamName { get; set; }
    
    public string? LogoLink { get; set; }
    
    public bool IsArchived { get; set; }
    
    public string GetCompleteTitle()
    {
        var result = $"{OrderTitle} ({Year})";
        
        if (!string.IsNullOrWhiteSpace(Title))
        {
            result += $" - {Title}";
        }
        
        return result;
    }
}