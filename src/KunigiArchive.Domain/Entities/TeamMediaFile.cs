namespace KunigiArchive.Domain.Entities;

public class TeamMediaFile
{
    public long TeamId { get; set; }
    
    public long MediaFileId { get; set; }

    public Team Team { get; set; } = null!;
    
    public MediaFile MediaFile { get; set; } = null!;
}