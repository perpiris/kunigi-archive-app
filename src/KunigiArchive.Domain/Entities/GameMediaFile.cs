namespace KunigiArchive.Domain.Entities;

public class GameMediaFile
{
    public long GameId { get; set; }
    
    public long MediaFileId { get; set; }

    public Game Game { get; set; } = null!;
    
    public MediaFile MediaFile { get; set; } = null!;
}