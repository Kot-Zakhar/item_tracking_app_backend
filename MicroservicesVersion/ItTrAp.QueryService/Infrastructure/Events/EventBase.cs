using MediatR;

namespace ItTrAp.QueryService.Infrastructure.Events;

public abstract record EventBase(string Type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}