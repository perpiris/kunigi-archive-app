namespace KunigiArchive.Domain.Entities;

public class MasterGameMediaFile
{
    public long MasterGameId { get; set; }
    
    public long MediaFileId { get; set; }

    public MasterGame MasterGame { get; set; } = null!;
    
    public MediaFile MediaFile { get; set; } = null!;
}