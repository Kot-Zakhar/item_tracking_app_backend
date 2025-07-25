using MediatR;

namespace ItTrAp.InventoryService.Application.Interfaces.Services;

public interface IEventPublishingService
{
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : INotification;
}