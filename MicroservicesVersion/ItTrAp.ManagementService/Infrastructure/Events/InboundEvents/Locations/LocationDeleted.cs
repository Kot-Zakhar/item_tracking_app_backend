namespace ItTrAp.ManagementService.Infrastructure.Events.InboundEvents.Locations;

public record LocationDeleted : EventBase
{
    public uint LocationId { get; init; }

    public LocationDeleted() : base(nameof(LocationDeleted))
    {
    }
}