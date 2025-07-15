using ItTrAp.LocationService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ItTrAp.LocationService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Location> Locations { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>()
            .Property(l => l.Id)
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Location>()
            .Property(u => u.Code)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Location>()
            .Property(l => l.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

    }
}