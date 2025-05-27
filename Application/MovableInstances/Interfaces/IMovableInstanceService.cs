namespace Application.MovableInstances.Interfaces;

public interface IMovableInstanceService
{
    Task<uint> CreateAsync(uint itemId, CancellationToken ct = default);
    Task DeleteAsync(uint itemId, uint id, CancellationToken ct = default);
}
