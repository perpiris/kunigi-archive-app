using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class GameMediaFileConfiguration : IEntityTypeConfiguration<GameMediaFile>
{
    public void Configure(EntityTypeBuilder<GameMediaFile> builder)
    {
        builder.HasKey(x => new { x.GameId, x.MediaFileId });
        
        builder.HasOne(x => x.Game)
            .WithMany(x => x.MediaFiles) 
            .HasForeignKey(x => x.GameId); 
        
        builder.HasOne(x => x.MediaFile)
            .WithMany(x => x.GameMediaFiles) 
            .HasForeignKey(x => x.MediaFileId); 
    }
}