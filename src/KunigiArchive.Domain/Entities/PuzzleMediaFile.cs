using KunigiArchive.Domain.Enums;

namespace KunigiArchive.Domain.Entities;

public class PuzzleMediaFile
{
    public long PuzzleId { get; set; }
    
    public long MediaFileId { get; set; }

    public PuzzleFileType FileType { get; set; }

    public Puzzle Puzzle { get; set; } = null!;
    
    public MediaFile MediaFile { get; set; } = null!;
}