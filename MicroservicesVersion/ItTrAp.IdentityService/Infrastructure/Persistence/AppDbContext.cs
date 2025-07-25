using ItTrAp.IdentityService.Constants;
using ItTrAp.IdentityService.Domain.Aggregates;
using ItTrAp.IdentityService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ItTrAp.IdentityService.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<User> Users { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        modelBuilder.Entity<User>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<User>()
            .Property("_passwordHash")
            .HasColumnName("password_hash")
            .IsRequired();
        modelBuilder.Entity<User>()
            .Property("_salt")
            .HasColumnName("salt")
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany<UserSession>()
            .WithOne(h => h.User)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        modelBuilder.Entity<User>()
            .HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity(j => j.ToTable("users_roles"));

        modelBuilder.Entity<UserSession>()
            .HasOne(u => u.User)
            .WithMany()
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        modelBuilder.Entity<UserSession>()
            .Property(u => u.RefreshToken)
            .HasValueGenerator<GuidValueGenerator>();
        modelBuilder.Entity<UserSession>()
            .Property(u => u.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

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