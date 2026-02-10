namespace ItTrAp.LocationService.Domain.Events.Locations;

public record LocationDeleted(uint LocationId) : EventBase(nameof(LocationDeleted));