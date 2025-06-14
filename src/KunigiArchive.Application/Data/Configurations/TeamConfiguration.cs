using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.HasKey(x => x.TeamId);
        builder.Property(x => x.TeamId).ValueGeneratedOnAdd();
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200)
            .UseCollation("el-GR-x-icu");
        
        builder.HasIndex(x => x.Slug).IsUnique();

        builder.Property(x => x.Description).HasMaxLength(4000);
        
        builder.Property(x => x.FacebookLink).HasMaxLength(500);
        builder.Property(x => x.InstagramLink).HasMaxLength(500);
        builder.Property(x => x.YoutubeLink).HasMaxLength(500);
        builder.Property(x => x.WebsiteLink).HasMaxLength(500);
    }
}