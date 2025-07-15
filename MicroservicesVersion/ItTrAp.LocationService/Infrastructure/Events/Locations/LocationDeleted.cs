namespace ItTrAp.LocationService.Infrastructure.Events.Locations;

public record LocationDeleted : EventBase
{
    public Guid LocationCode { get; }

    public LocationDeleted(Guid locationCode)
        : base(nameof(LocationDeleted))
    {
        LocationCode = locationCode;
    }    
}