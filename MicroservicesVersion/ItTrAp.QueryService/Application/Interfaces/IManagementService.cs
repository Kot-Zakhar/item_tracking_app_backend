using ItTrAp.QueryService.Domain.Enums;
using ItTrAp.QueryService.Application.DTOs;

namespace ItTrAp.QueryService.Application.Interfaces.Services;

// TODO: use interfaces of collections instead of concrete types
public interface IManagementService
{
    Task<List<uint>> GetInstanceAmountsInLocationsAsync(List<uint> locationIds, CancellationToken cancellationToken = default);
    Task<IList<MovableInstanceStatusDto>> GetInstanceStatusesByItemAsync(Guid movableItemId, CancellationToken cancellationToken);
    Task<Dictionary<Guid, List<KeyValuePair<MovableInstanceStatus, uint>>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default);
    Task<IList<uint>> GetItemAmountsByUserIdsAsync(List<uint> userIds, CancellationToken cancellationToken = default);
    Task<IList<MovableInstanceStatusDto>> GetFilteredMovableInstancesAsync(List<uint>? userIds, List<uint>? locationIds, MovableInstanceStatus? status, CancellationToken cancellationToken = default);
}