namespace ItTrAp.ManagementService.Domain.Aggregates;

public class MovableItem
{
    public Guid Id { get; set; }

    public virtual IList<MovableInstance> MovableInstances { get; set; } = new List<MovableInstance>();

    public static MovableItem Create(Guid id)
    {
        return new MovableItem
        {
            Id = id
        };
    }

    public MovableInstance QuickAddInstance()
    {
        var instance = MovableInstance.Create(this);

        MovableInstances.Add(instance);

        return instance;
    }

}