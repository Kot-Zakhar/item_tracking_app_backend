namespace ItTrAp.ManagementService.Infrastructure.Interfaces.Services;

public interface IInboundEventService
{
    Task ProcessInboundEventAsync(string messageId, string eventData, CancellationToken stoppingToken = default);
}