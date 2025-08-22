using ItTrAp.ManagementService.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ItTrAp.ManagementService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<MovableInstance> MovableInstances { get; set; }
    public DbSet<MovableItem> MovableItems { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<User> Users { get; set; } 

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovableInstance>()
            .HasKey(mi => mi.Id);

        modelBuilder.Entity<MovableInstance>()
            .Property(mi => mi.Code)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.Location)
            .WithMany(l => l.MovableInstances);
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.User)
            .WithMany(u => u.MovableInstances);
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.MovableItem)
            .WithMany(mi => mi.MovableInstances)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovableItem>()
            .HasKey(mi => mi.Id);
        modelBuilder.Entity<MovableItem>()
            .HasMany(mi => mi.MovableInstances)
            .WithOne(i => i.MovableItem)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Location>()
            .HasKey(l => l.Id);
        modelBuilder.Entity<Location>()
            .Property(mi => mi.Code)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Location>()
            .HasMany(l => l.MovableInstances)
            .WithOne(mi => mi.Location)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<User>()
            .HasMany(u => u.MovableInstances)
            .WithOne(mi => mi.User)
            .OnDelete(DeleteBehavior.Restrict);
    }
}