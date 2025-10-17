using ItTrAp.QueryService.Domain.Enums;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

public interface IManagementService
{
    Task<List<uint>> GetInstanceAmountsInLocationsAsync(List<uint> locationIds, CancellationToken cancellationToken = default);

    Task<Dictionary<Guid, List<KeyValuePair<MovableInstanceStatus, uint>>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default);
}