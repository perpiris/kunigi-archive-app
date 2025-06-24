using KunigiArchive.Web.ViewModels.Common;

namespace KunigiArchive.Web.ViewModels.Team;

public class TeamMediaDetailsViewModel
{
    public long TeamId { get; set; }
    
    public required string TeamName { get; set; }
    
    public required string Slug { get; set; }
    
    public List<MediaFileViewModel> MediaFiles { get; set; } = [];
}