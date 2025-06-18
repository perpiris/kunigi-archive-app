using KunigiArchive.Domain.Entities;
using KunigiArchive.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class PuzzleMediaFileConfiguration : IEntityTypeConfiguration<PuzzleMediaFile>
{
    public void Configure(EntityTypeBuilder<PuzzleMediaFile> builder)
    {
        builder.HasKey(x => new { x.PuzzleId, x.MediaFileId, x.FileType });
        
        builder.Property(e => e.FileType)
            .HasConversion(
                v => v.ToString(),
                v => Enum.Parse<PuzzleFileType>(v))
            .HasMaxLength(50);
        
        builder.HasOne(x => x.Puzzle)
            .WithMany(x => x.MediaFiles) 
            .HasForeignKey(x => x.PuzzleId); 
        
        builder.HasOne(x => x.MediaFile)
            .WithMany(x => x.PuzzleMediaFiles) 
            .HasForeignKey(x => x.MediaFileId); 
    }
}