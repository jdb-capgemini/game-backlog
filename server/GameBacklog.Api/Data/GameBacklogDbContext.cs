using GameBacklog.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameBacklog.Api.Data;

public class GameBacklogDbContext : DbContext
{
    public GameBacklogDbContext(
        DbContextOptions<GameBacklogDbContext> options)
        : base(options)
    {
    }

    public DbSet<CatalogGame> CatalogGames => Set<CatalogGame>();

    public DbSet<BacklogEntry> BacklogEntries => Set<BacklogEntry>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CatalogGame>(entity =>
        {
            entity.HasIndex(game => game.RawgId)
                .IsUnique();

            entity.HasIndex(game => game.Name);

            entity.HasMany(game => game.BacklogEntries)
                .WithOne(entry => entry.CatalogGame)
                .HasForeignKey(entry => entry.CatalogGameId);
        });

        modelBuilder.Entity<BacklogEntry>(entity =>
        {
            entity.HasIndex(entry => new
                {
                    entry.UserId,
                    entry.CatalogGameId
                })
                .IsUnique();

            entity.Property(entry => entry.Status)
                .HasConversion<string>();

            entity.Property(entry => entry.EstimatedHours)
                .HasPrecision(8, 2);

            entity.HasIndex(entry => entry.Status);
        });

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.HasIndex(user => user.GoogleSubjectId).IsUnique();

            entity.HasMany(user => user.BacklogEntries).WithOne(entry => entry.User)
                .HasForeignKey(entry => entry.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BacklogEntry>(entity =>
        {
            entity.HasIndex(entry => new
            {
                entry.UserId,
                entry.CatalogGameId
                
            }).IsUnique();

            entity.Property(entry => entry.Status).HasConversion<string>();
            
            entity.Property(entry => entry.EstimatedHours).HasPrecision(8, 2);
            
            entity.HasIndex(entry => entry.Status);
        });
    }
}