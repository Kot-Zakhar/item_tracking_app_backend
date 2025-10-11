namespace ItTrAp.QueryService.Infrastructure.Events.InboundEvents;

public record LocationDeleted : EventBase
{
    public uint LocationId { get; init; }

    public LocationDeleted() : base(nameof(LocationDeleted))
    {
    }
}