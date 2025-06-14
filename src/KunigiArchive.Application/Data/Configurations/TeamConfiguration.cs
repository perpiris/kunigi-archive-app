using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(t => t.TeamId);
        builder.Property(t => t.TeamId).ValueGeneratedOnAdd();
        
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200)
            .UseCollation("el-GR-x-icu");
        
        builder.HasIndex(t => t.Slug).IsUnique();

        builder.Property(t => t.Description).HasMaxLength(4000);
        
        builder.Property(t => t.FacebookLink).HasMaxLength(500);
        builder.Property(t => t.InstagramLink).HasMaxLength(500);
        builder.Property(t => t.YoutubeLink).HasMaxLength(500);
        builder.Property(t => t.WebsiteLink).HasMaxLength(500);
    }
}