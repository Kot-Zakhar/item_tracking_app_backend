namespace ItTrAp.InventoryService.Application.Interfaces.Services;

public interface IMovableInstanceService
{
    Task<uint> CreateAsync(Guid itemId, CancellationToken ct = default);
    Task DeleteAsync(Guid itemId, uint id, CancellationToken ct = default);
}
