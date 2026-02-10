namespace ItTrAp.ManagementService.Application.Interfaces.Services;

public interface IMovableInstanceService
{
    Task CreateAsync(Guid itemId, uint id, CancellationToken ct = default);
    Task DeleteAsync(Guid itemId, uint id, CancellationToken ct = default);
}
