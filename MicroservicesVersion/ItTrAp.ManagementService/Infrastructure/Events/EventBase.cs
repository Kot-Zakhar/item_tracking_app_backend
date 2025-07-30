using MediatR;

namespace ItTrAp.ManagementService.Infrastructure.Events;

public abstract record EventBase(string Type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}