using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class MasterGameMediaFileConfiguration : IEntityTypeConfiguration<MasterGameMediaFile>
{
    public void Configure(EntityTypeBuilder<MasterGameMediaFile> builder)
    {
        builder.HasKey(x => new { x.MasterGameId, x.MediaFileId });
        
        builder.HasOne(x => x.MasterGame)
            .WithMany(x => x.MediaFiles) 
            .HasForeignKey(x => x.MasterGameId); 
        
        builder.HasOne(x => x.MediaFile)
            .WithMany(x => x.MasterGameMediaFiles) 
            .HasForeignKey(x => x.MediaFileId); 
    }
}