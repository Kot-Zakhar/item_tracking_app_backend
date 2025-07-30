namespace ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;

public record MovableItemCreated : EventBase
{
    public Guid MovableItemId { get; set; }

    public MovableItemCreated() : base(nameof(MovableItemCreated))
    {
    }
}