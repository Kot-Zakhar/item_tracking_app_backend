using MediatR;

namespace ItTrAp.UserService.Application.Interfaces.Services;

public interface IEventPublishingService
{
    Task PublishAsync<TEvent>(TEvent eventMessage, CancellationToken cancellationToken = default) where TEvent : INotification;
}