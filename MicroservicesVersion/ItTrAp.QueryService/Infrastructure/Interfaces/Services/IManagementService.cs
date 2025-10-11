namespace ItTrAp.QueryService.Infrastructure.Interfaces.Service;

public interface IManagementService
{
    Task<List<uint>> GetInstanceAmountsInLocationsAsync(List<uint> locationIds, CancellationToken cancellationToken = default);
}