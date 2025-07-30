using MediatR;

namespace ItTrAp.IdentityService.Infrastructure.Events;

public abstract record EventBase(string Type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}