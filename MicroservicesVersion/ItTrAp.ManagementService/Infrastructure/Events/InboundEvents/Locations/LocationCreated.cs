namespace ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;

public record LocationCreated : EventBase
{
    public uint LocationId { get; init; }
    public Guid LocationCode { get; init; }

    public LocationCreated() : base(nameof(LocationCreated))
    {
    }
}