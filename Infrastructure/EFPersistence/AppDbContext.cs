using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Domain.Models;
using Infrastructure.Constants;

namespace Infrastructure.EFPersistence;

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
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

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
            .HasColumnName("password_hash")
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property("_salt")
            .HasColumnName("salt")
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany(u => u.HistoryOfReservations)
            .WithOne(h => h.User)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("users_roles"));

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
        modelBuilder.Entity<MovableInstance>()
            .HasMany(i => i.History)
            .WithOne(h => h.MovableInstance)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MovableInstanceHistory>()
            .HasOne(h => h.MovableInstance)
            .WithMany(i => i.History)
            .IsRequired();
        modelBuilder.Entity<MovableInstanceHistory>()
            .HasOne(h => h.User)
            .WithMany(u => u.HistoryOfReservations)
            .IsRequired();
        modelBuilder.Entity<MovableInstanceHistory>()
            .HasOne(h => h.FromLocation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
        modelBuilder.Entity<MovableInstanceHistory>()
            .HasOne(h => h.ToLocation)
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        modelBuilder.Entity<Role>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Users)
            .WithMany(u => u.Roles)
            .UsingEntity(j => j.ToTable("users_roles"));
        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("roles_permissions"));

        modelBuilder.Entity<Permission>()
            .Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        modelBuilder.Entity<Permission>()
            .HasMany(p => p.Roles)
            .WithMany(r => r.Permissions)
            .UsingEntity(j => j.ToTable("roles_permissions"));
        modelBuilder.Entity<Permission>()
            .HasIndex(p => p.Name)
            .IsUnique();


        // Seed permissions
        foreach (var permission in SecurityConstants.Permissions.AllPermissions)
        {
            modelBuilder.Entity<Permission>()
                .HasData(new Permission
                {
                    Id = permission.Id,
                    Name = permission.Name
                });
        }

        // Seed roles
        foreach (var role in SecurityConstants.Roles.PredefinedRoles)
        {
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = role.Id,
                    Name = role.Name
                });
        }

        // Seed role-permission relationships
        var rolePermissionData = new List<Dictionary<string, object>>();
        
        foreach (var rolePermission in SecurityConstants.RolePermissions)
        {
            var roleId = SecurityConstants.Roles.PredefinedRoles
                .First(r => r.Name == rolePermission.Key).Id;
            
            foreach (var permissionName in rolePermission.Value)
            {
                var permissionId = SecurityConstants.Permissions.AllPermissions
                    .First(p => p.Name == permissionName).Id;
                    
                rolePermissionData.Add(new Dictionary<string, object>
                {
                    { "RolesId", roleId },
                    { "PermissionsId", permissionId }
                });
            }
        }

        modelBuilder.Entity<Role>()
            .HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => 
            {
                j.ToTable("roles_permissions");
                j.HasData(rolePermissionData);
            });
    }
}