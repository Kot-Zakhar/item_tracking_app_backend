namespace Application.Reservations.Interfaces;

public interface IReservationService
{
    Task<uint> BookAnyInstanceInLocationAsync(uint issuerId, uint userId, uint itemId, uint locationId, CancellationToken ct = default);
    Task BookAsync(uint issuerId, uint bookerId, uint instanceId, CancellationToken ct = default);
    Task CancelBookingAsync(uint issuerId, uint instanceId, CancellationToken ct = default);
    Task AssignAsync(uint issuerId, uint assigneeId, uint instanceId, CancellationToken ct = default);
    Task TakeByCodeAsync(uint issuerId, Guid code, CancellationToken ct = default);
    Task ReleaseForcefullyAsync(uint issuerId, uint instanceId, uint locationId, CancellationToken ct = default);
    Task ReleaseAsync(uint issuerId, Guid instanceCode, Guid locationCode, CancellationToken ct = default);
    Task MoveAsync(uint issuerId, uint instanceId, uint locationId, CancellationToken ct = default);
}