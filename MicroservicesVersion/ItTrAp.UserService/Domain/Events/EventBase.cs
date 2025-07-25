using MediatR;

namespace ItTrAp.UserService.Domain.Events;

public abstract record EventBase(string Type) : INotification
{
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}