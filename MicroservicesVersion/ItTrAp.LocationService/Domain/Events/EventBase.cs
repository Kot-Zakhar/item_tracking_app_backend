using MediatR;

namespace ItTrAp.LocationService.Domain.Events;

public abstract record EventBase(string Type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}