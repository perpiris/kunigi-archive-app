using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace KunigiArchive.Application.Data.Configurations;
public class GameConfiguration : IEntityTypeConfiguration<Game>
{
    public void Configure(EntityTypeBuilder<Game> builder)
    {
        builder.HasKey(x => x.GameId);
        builder.Property(x => x.GameId).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Description).HasMaxLength(4000);
        
        builder.Property(x => x.Title).HasMaxLength(500);
        builder.Property(x => x.SubTitle).HasMaxLength(500);
        
        builder.Property(x => x.LogoLink).HasMaxLength(500);
        
        builder.HasOne(x => x.GameType)
            .WithMany(x => x.Games)
            .HasForeignKey(x => x.GameTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}