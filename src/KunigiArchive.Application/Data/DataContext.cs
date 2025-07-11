﻿using KunigiArchive.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KunigiArchive.Application.Data;

public class DataContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
{
    public new DbSet<ApplicationUser> Users { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamManager> TeamManagers { get; set; }
    public DbSet<MasterGame> MasterGames { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<GameType> GameTypes { get; set; }
    public DbSet<Puzzle> Puzzles { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }
    public DbSet<TeamMediaFile> TeamMediaFiles { get; set; }
    public DbSet<MasterGameMediaFile> MasterGameMediaFiles { get; set; }
    public DbSet<GameMediaFile> GameMediaFiles { get; set; }
    public DbSet<PuzzleMediaFile> PuzzleMediaFiles { get; set; }
    
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
    }
}