using ItTrAp.InventoryService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

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
            .HasKey(c => c.Id);
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
            .HasMany(i => i.MovableItems)
            .WithOne(i => i.Category)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        modelBuilder.Entity<MovableItem>()
            .HasKey(i => i.Id);
        modelBuilder.Entity<MovableItem>()
            .Property(i => i.Id)
            .HasValueGenerator<GuidValueGenerator>()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<MovableItem>()
            .Property(l => l.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<MovableItem>()
            .Ignore(i => i.Name);
        modelBuilder.Entity<MovableItem>()
            .Ignore(i => i.Description);
        // modelBuilder.Entity<MovableItem>()
        //     .Ignore(i => i.CategoryId);
        modelBuilder.Entity<MovableItem>()
            .Ignore(i => i.ImgSrc);
        modelBuilder.Entity<MovableItem>()
            .Ignore(i => i.ExtraData);
        modelBuilder.Entity<MovableItem>()
            .HasOne(i => i.Category)
            .WithMany(i => i.MovableItems)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
    }
}