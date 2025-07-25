namespace ItTrAp.IdentityService.Domain.Aggregates;

public class Permission
{
    public uint Id { get; set; }
    public required string Name { get; set; }
    public virtual List<Role> Roles { get; set; } = new();

    public override string ToString() => Name;
}