namespace ItTrAp.InventoryService.Domain.Aggregates;

public class MovableInstance
{
    public uint Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public virtual required MovableItem MovableItem { get; set; }
}
