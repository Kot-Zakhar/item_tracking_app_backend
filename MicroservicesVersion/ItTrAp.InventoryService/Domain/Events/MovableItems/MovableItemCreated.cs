namespace ItTrAp.InventoryService.Domain.Events.MovableItems;

public record MovableItemCreated : EventBase
{
    public MovableItemCreated() : base(nameof(MovableItemCreated)) { }

    public Guid MovableItemId { get; init; }
}