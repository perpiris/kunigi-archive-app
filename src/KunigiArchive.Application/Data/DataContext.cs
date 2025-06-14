﻿using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KunigiArchive.Application.Data;

public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamManager> TeamManagers { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}