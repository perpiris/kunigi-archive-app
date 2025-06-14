using KunigiArchive.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KunigiArchive.Application.Data.Configurations;

public class TeamManagerConfiguration : IEntityTypeConfiguration<TeamManager>
{
    public void Configure(EntityTypeBuilder<TeamManager> builder)
    {
        builder.HasKey(x => new { x.TeamId, x.ApplicationUserId });
        
        builder.HasOne(x => x.Team)
            .WithMany(x => x.Managers) 
            .HasForeignKey(x => x.TeamId); 
        
        builder.HasOne(x => x.ApplicationUser)
            .WithMany(x => x.ManagedTeams) 
            .HasForeignKey(x => x.ApplicationUserId); 
    }
}