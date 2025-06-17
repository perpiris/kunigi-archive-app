namespace KunigiArchive.Domain.Entities;

public class GameType
{
    public long GameTypeId { get; set; }

    public required string Label { get; set; }

    public required string Slug { get; set; }
    
    public ICollection<Game> Games { get; set; } = new List<Game>();
}