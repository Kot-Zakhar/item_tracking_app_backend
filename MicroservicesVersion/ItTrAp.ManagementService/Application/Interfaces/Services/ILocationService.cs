namespace ItTrAp.ManagementService.Application.Interfaces.Services;

public interface ILocationService
{
    Task CreateAsync(uint locationId, Guid locationCode, CancellationToken cancellationToken);
    Task DeleteAsync(uint locationId, CancellationToken cancellationToken);
}