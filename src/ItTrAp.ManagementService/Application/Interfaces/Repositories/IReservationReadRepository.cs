using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.DTOs.Reservations;

namespace ItTrAp.ManagementService.Application.Interfaces.Repositories;
// TODO: use collection interfaces instead of concrete types
public interface IReservationReadRepository
{
    Task<List<MovableInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken);
    Task<IList<InstanceStatusDto>> GetInstanceStatusesByItemIdAsync(Guid itemId, CancellationToken cancellationToken);
    Task<Dictionary<Guid, List<UserStatusDto>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken);
    Task<IList<uint>> GetItemAmountsByUserIdsAsync(IList<uint> userIds, CancellationToken cancellationToken);
}