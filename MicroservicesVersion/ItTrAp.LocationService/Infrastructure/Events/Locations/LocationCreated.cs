namespace ItTrAp.LocationService.Infrastructure.Events.Locations;

public record LocationCreated : EventBase
{
    public Guid LocationCode { get; }
    public string Name { get; }

    public LocationCreated(Guid locationCode, string name)
        : base(nameof(LocationCreated))
    {
        LocationCode = locationCode;
        Name = name;
    }    
}