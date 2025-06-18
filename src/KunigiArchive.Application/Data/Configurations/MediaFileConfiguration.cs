using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class MediaFileConfiguration : IEntityTypeConfiguration<MediaFile>
{
    public void Configure(EntityTypeBuilder<MediaFile> builder)
    {
        builder.HasKey(x => x.MediaFileId);
        builder.Property(x => x.MediaFileId).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.Path)
            .IsRequired()
            .HasMaxLength(2000);
    }
}