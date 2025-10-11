namespace ItTrAp.QueryService.Infrastructure.Events.InboundEvents;

public record LocationCreated : EventBase
{
    public uint LocationId { get; init; }

    public LocationCreated() : base(nameof(LocationCreated))
    {
    }
}