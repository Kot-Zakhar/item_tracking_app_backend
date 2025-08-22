namespace ItTrAp.InventoryService.Domain.Events.MovableInstances;

public record MovableInstanceCreated : EventBase
{
    public MovableInstanceCreated() : base(nameof(MovableInstanceCreated)) { }

    public Guid MovableItemId { get; init; }
    public uint MovableInstanceId { get; init; }
}