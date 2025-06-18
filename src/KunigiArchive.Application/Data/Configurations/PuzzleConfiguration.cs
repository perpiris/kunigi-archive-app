using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class PuzzleConfiguration : IEntityTypeConfiguration<Puzzle>
{
    public void Configure(EntityTypeBuilder<Puzzle> builder)
    {
        builder.HasKey(x => x.PuzzleId);
        builder.Property(x => x.PuzzleId).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Question).HasMaxLength(4000);
        builder.Property(x => x.Answer).HasMaxLength(4000);

        builder.HasOne(x => x.Game)
            .WithMany(x => x.Puzzles)
            .HasForeignKey(x => x.GameId);
    }
}