using MediatR;

namespace ItTrAp.LocationService.Infrastructure.Events;

public abstract record EventBase(string type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public string Type { get; } = type;
}