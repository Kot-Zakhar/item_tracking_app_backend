namespace ItTrAp.InventoryService.Domain.Events.MovableItems;

public record MovableItemDeleted : EventBase
{
    public MovableItemDeleted() : base(nameof(MovableItemDeleted)) { }

    public Guid MovableItemId { get; init; }
}