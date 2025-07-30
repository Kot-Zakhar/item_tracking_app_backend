namespace ItTrAp.ManagementService.Application.Interfaces.Services;

public interface IMovableInstanceService
{
    Task<uint> CreateAsync(Guid itemId, uint issuerId, CancellationToken ct = default);
    Task DeleteAsync(Guid itemId, uint id, uint issuerId, CancellationToken ct = default);
}
