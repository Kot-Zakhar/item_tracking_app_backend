using Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Infrastructure.EventHandlers;

public class EventLogger<TDomainEvent> (ILogger<EventLogger<TDomainEvent>> logger) : INotificationHandler<TDomainEvent>
    where TDomainEvent : DomainEvent
{
    public Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        var eventType = notification.GetType().Name;
        logger.LogInformation("Registered event: {EventType}", eventType);

        return Task.CompletedTask;
    }
}