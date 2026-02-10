using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Infrastructure.Events.InboundEvents;

public record MovableInstanceState
{
    public MovableInstanceStatus Status;
    public uint? UserId;
    public uint? LocationId;
}

public record MovableInstanceStatusChanged : EventBase
{
    public MovableInstanceStatusChanged() : base(nameof(MovableInstanceStatusChanged)) { }

    public Guid MovableItemId { get; init; }
    public uint MovableInstanceId { get; init; }
    public required MovableInstanceState Before { get; init; }
    public required MovableInstanceState After { get; init; }
}