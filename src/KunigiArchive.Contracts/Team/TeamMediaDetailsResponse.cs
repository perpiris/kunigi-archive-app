using KunigiArchive.Contracts.MediaFile;

namespace KunigiArchive.Contracts.Team;

public class TeamMediaDetailsResponse
{
    public long TeamId { get; set; }
    
    public required string TeamName { get; set; }
    
    public required string Slug { get; set; }
    
    public List<MediaFileResponse> MediaFiles { get; set; } = [];
}