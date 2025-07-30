using ItTrAp.ManagementService.Application.DTOs.MovableInstances;

namespace ItTrAp.ManagementService.Application.Interfaces.Repositories;

public interface IReservationReadRepository
{
    Task<List<MovableInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken);
}