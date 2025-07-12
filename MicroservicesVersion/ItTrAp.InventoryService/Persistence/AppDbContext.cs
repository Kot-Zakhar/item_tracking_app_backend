using ItTrAp.InventoryService.Models;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.InventoryService.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<MovableItem> MovableItems { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired();
        modelBuilder.Entity<Category>()
            .Property(c => c.Icon)
            .HasMaxLength(100);
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Children)
            .WithOne(c => c.Parent)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Category>()
            .HasMany<MovableItem>()
            .WithOne(i => i.Category)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MovableItem>()
            .Property(l => l.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<MovableItem>()
            .Property(l => l.Name)
            .IsRequired();
        modelBuilder.Entity<MovableItem>()
            .HasOne(i => i.Category)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}