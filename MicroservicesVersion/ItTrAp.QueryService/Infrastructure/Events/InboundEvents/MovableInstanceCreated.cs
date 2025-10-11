namespace ItTrAp.QueryService.Infrastructure.Events.InboundEvents;

public record MovableInstanceCreated : EventBase
{
    public MovableInstanceCreated() : base(nameof(MovableInstanceCreated)) { }

    public Guid MovableItemId { get; init; }
    public uint MovableInstanceId { get; init; }
}