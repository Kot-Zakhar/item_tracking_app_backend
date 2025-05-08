using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Domain.Users;
using Domain.MovableItems;
using Domain.Locations;
using Domain.Categories;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<MovableItem> MovableItems { get; set; }
    public DbSet<MovableInstance> MovableInstances { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Phone)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<User>()
            .HasMany(u => u.MovableInstances)
            .WithOne(i => i.User);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Sessions)
            .WithOne(s => s.User);
        modelBuilder.Entity<User>()
            .Property("_passwordHash")
            .HasColumnName("PasswordHash")
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property("_salt")
            .HasColumnName("Salt")
            .IsRequired();

        modelBuilder.Entity<UserSession>()
            .HasOne(u => u.User)
            .WithMany(u => u.Sessions)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        modelBuilder.Entity<UserSession>()
            .Property(u => u.RefreshToken)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<UserSession>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        modelBuilder.Entity<Location>()
            .Property(u => u.Code)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<Location>()
            .Property(l => l.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Location>()
            .HasMany(l => l.Instances)
            .WithOne(i => i.Location)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Category>()
            .Property(c => c.Name)
            .IsRequired();
        modelBuilder.Entity<Category>()
            .HasMany(c => c.Children)
            .WithOne(c => c.Parent)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Category>()
            .HasMany(c => c.MovableItems)
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
            .WithMany(i => i.MovableItems)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        modelBuilder.Entity<MovableItem>()
            .HasMany(i => i.Instances)
            .WithOne(i => i.MovableItem)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<MovableInstance>()
            .Property(u => u.Code)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<MovableInstance>()
            .Property(l => l.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.MovableItem)
            .WithMany(i => i.Instances)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.Location)
            .WithMany(i => i.Instances)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MovableInstance>()
            .HasOne(i => i.User)
            .WithMany(i => i.MovableInstances)
            .OnDelete(DeleteBehavior.Restrict);
        
    }
}