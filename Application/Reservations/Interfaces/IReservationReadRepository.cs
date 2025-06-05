using Application.Reservations.DTOs;

namespace Application.Reservations.Interfaces;

public interface IReservationReadRepository
{
    Task<List<ItemInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken);
}