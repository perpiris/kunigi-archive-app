using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class GameTypeConfiguration : IEntityTypeConfiguration<GameType>
{
    public void Configure(EntityTypeBuilder<GameType> builder)
    {
        builder.HasKey(x => x.GameTypeId);
        builder.Property(x => x.GameTypeId).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Label)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.HasIndex(x => x.Slug).IsUnique();
    }
}