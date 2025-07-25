namespace ItTrAp.LocationService.Domain.Events.Locations;

public record LocationCreated(uint LocationId, Guid LocationCode) : EventBase(nameof(LocationCreated));