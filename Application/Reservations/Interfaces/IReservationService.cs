using Domain.MovableItems;

namespace Application.Reservations.Interfaces;

public interface IReservationService
{
    Task<uint> BookAnyInstanceInLocationAsync(uint userId, uint itemId, uint locationId, CancellationToken ct = default);
    Task BookAsync(uint userId, uint instanceId, CancellationToken ct = default);
    Task CancelBookingAsync(uint userId, uint instanceId, CancellationToken ct = default);
    Task TakeByCodeAsync(uint userId, Guid code, CancellationToken ct = default);
    Task ReleaseAsync(uint userId, uint instanceId, uint locationId, CancellationToken ct = default);
    Task ReleaseAsync(uint userId, Guid instanceCode, Guid locationCode, CancellationToken ct = default);
    Task TakeAsync(uint userId, uint instanceId, CancellationToken ct = default);
    Task MoveOrReleaseAsync(uint userId, uint instanceId, uint? locationId, CancellationToken ct = default);
}