using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class TeamMediaFileConfiguration : IEntityTypeConfiguration<TeamMediaFile>
{
    public void Configure(EntityTypeBuilder<TeamMediaFile> builder)
    {
        builder.HasKey(x => new { x.TeamId, x.MediaFileId });
        
        builder.HasOne(x => x.Team)
            .WithMany(x => x.MediaFiles) 
            .HasForeignKey(x => x.TeamId); 
        
        builder.HasOne(x => x.MediaFile)
            .WithMany(x => x.TeamMediaFiles) 
            .HasForeignKey(x => x.MediaFileId); 
    }
}