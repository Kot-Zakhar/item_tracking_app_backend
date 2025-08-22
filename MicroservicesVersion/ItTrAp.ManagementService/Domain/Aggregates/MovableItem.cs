namespace ItTrAp.ManagementService.Domain.Aggregates;

public class MovableItem
{
    public Guid Id { get; set; }

    public virtual IList<MovableInstance> MovableInstances { get; set; } = [];

    public static MovableItem Create(Guid id)
    {
        return new MovableItem
        {
            Id = id
        };
    }
}