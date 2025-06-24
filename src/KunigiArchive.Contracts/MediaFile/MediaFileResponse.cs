namespace KunigiArchive.Contracts.MediaFile;

public class MediaFileResponse
{
    public long MediaFileId { get; set; }
    
    public required string FileName { get; set; }

    public required string Path { get; set; }
}