namespace KunigiArchive.Domain.Entities;

public class MediaFile
{
    public long MediaFileId { get; set; }

    public required string Name { get; set; }

    public required string Path { get; set; }
    
    public ICollection<TeamMediaFile> TeamMediaFiles { get; set; } = new List<TeamMediaFile>();
    
    public ICollection<MasterGameMediaFile> MasterGameMediaFiles { get; set; } = new List<MasterGameMediaFile>();
    
    public ICollection<GameMediaFile> GameMediaFiles { get; set; } = new List<GameMediaFile>();
    
    public ICollection<PuzzleMediaFile> PuzzleMediaFiles { get; set; } = new List<PuzzleMediaFile>();
}