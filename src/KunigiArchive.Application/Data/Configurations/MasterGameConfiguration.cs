using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class MasterGameConfiguration : IEntityTypeConfiguration<MasterGame>
{
    public void Configure(EntityTypeBuilder<MasterGame> builder)
    {
        builder.HasKey(x => x.MasterGameId);
        builder.Property(x => x.MasterGameId).ValueGeneratedOnAdd();

        builder.Property(x => x.Year).IsRequired();
        builder.Property(x => x.Order).IsRequired();

        builder.HasIndex(x => x.Year).IsUnique();
        
        builder.Property(x => x.Description).HasMaxLength(4000);
        
        builder.Property(x => x.Title).HasMaxLength(500);
        builder.Property(x => x.OrderTitle).IsRequired().HasMaxLength(500);
        
        builder.Property(x => x.LogoLink).HasMaxLength(500);

        builder.HasOne(x => x.HostTeam)
            .WithMany()
            .HasForeignKey(x => x.HostTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.WinnerTeam)
            .WithMany()
            .HasForeignKey(x => x.WinnerTeamId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(x => x.Games)
            .WithOne(x => x.MasterGame)
            .HasForeignKey(x => x.MasterGameId);
    }
}