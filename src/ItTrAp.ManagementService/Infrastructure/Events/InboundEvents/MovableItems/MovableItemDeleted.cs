namespace ItTrAp.ManagementService.Infrastructure.Events.InboundEvents;

public record MovableItemDeleted : EventBase
{
    public Guid MovableItemId { get; set; }
    public MovableItemDeleted() : base(nameof(MovableItemDeleted))
    {
    }
}