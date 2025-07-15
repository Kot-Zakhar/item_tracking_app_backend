using ItTrAp.LocationService.Infrastructure.Events;
using MediatR;

namespace ItTrAp.LocationService.Infrastructure.EventHandlers;

public class MessageBusPublisher<TEvent> : INotificationHandler<TEvent>
    where TEvent : EventBase
{
    // private readonly IMessageBus _messageBus;
    private readonly ILogger<MessageBusPublisher<TEvent>> _logger;

    public MessageBusPublisher(
        // IMessageBus messageBus,
        ILogger<MessageBusPublisher<TEvent>> logger)
    {
        // _messageBus = messageBus;
        _logger = logger;
    }

    public Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        // await _messageBus.PublishAsync(notification, cancellationToken);

        _logger.LogInformation("Publishing event: {EventType}", notification.Type);

        return Task.CompletedTask;
    }
}