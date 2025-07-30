namespace ItTrAp.ManagementService.Domain.Aggregates;

public class User
{
    public static readonly int MaxMovableInstances = 10;
    public uint Id { get; set; }
    public required string Email { get; set; }

    public virtual IList<MovableInstance> MovableInstances { get; set; } = new List<MovableInstance>();

    public static User Create(uint id, string email)
    {
        return new User
        {
            Id = id,
            Email = email
        };
    }

    public bool IsMaxMovableInstancesReached() => MovableInstances.Count == MaxMovableInstances;
}