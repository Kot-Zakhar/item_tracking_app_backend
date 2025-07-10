using MediatR;

namespace Domain.Events;

public abstract record DomainEvent : INotification
{
    DateTime Timestamp { get; } = DateTime.UtcNow;
}