using MediatR;

namespace ItTrAp.InventoryService.Infrastructure.Interfaces.Services;

public interface IEventPublishingService
{
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : INotification;
}