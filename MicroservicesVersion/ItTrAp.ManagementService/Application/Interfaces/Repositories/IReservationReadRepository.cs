using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.DTOs.Reservations;

namespace ItTrAp.ManagementService.Application.Interfaces.Repositories;

public interface IReservationReadRepository
{
    Task<List<MovableInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken);
    Task<Dictionary<Guid, List<UserStatusDto>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken);
}