namespace KunigiArchive.Domain.Entities;

public class Game
{
    public long GameId { get; set; }

    public long MasterGameId { get; set; }
    
    public string? Title { get; set; }
    
    public string? SubTitle { get; set; }
    
    public string? Description { get; set; }
    
    public string? LogoLink { get; set; }
    
    public long GameTypeId { get; set; }

    public MasterGame MasterGame { get; set; } = null!;

    public GameType GameType { get; set; } = null!;
    
    public ICollection<Puzzle> Puzzles { get; set; } = new List<Puzzle>();
    
    public ICollection<GameMediaFile> MediaFiles { get; set; } = new List<GameMediaFile>();
}