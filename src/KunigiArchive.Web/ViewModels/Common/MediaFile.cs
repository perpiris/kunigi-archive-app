namespace KunigiArchive.Web.ViewModels.Common;

public class MediaFileViewModel
{
    public long MediaFileId { get; set; }
    
    public required string FileName { get; set; }

    public required string Path { get; set; }
}