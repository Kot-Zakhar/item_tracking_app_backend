namespace ItTrAp.QueryService.Infrastructure.Events.InboundEvents;

public record MovableInstanceDeleted : EventBase
{
    public MovableInstanceDeleted() : base(nameof(MovableInstanceDeleted)) { }

    public Guid MovableItemId { get; init; }
    public uint MovableInstanceId { get; init; }
}