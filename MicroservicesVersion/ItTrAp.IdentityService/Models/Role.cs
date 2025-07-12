namespace ItTrAp.IdentityService.Models;

public class Role
{
    public static readonly string AdminRoleName = "admin";
    public uint Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }

    public virtual List<Permission> Permissions { get; set; } = new();
    public virtual List<User> Users { get; set; } = new();

    public override string ToString() => $"{Name} ({Description})";
}