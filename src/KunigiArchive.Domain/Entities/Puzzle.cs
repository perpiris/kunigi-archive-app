namespace KunigiArchive.Domain.Entities;

public class Puzzle
{
    public long PuzzleId { get; set; }
    
    public required string Question { get; set; }
    
    public required string Answer { get; set; }
    
    public long GameId { get; set; }

    public Game Game { get; set; } = null!;
    
    public ICollection<PuzzleMediaFile> MediaFiles { get; set; } = new List<PuzzleMediaFile>();
}