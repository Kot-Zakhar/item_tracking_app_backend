namespace ItTrAp.LocationService.Domain.Events.Locations;

public record LocationCreated(uint LocationId) : EventBase(nameof(LocationCreated));