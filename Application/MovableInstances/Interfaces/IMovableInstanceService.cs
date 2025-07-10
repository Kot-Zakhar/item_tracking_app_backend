namespace Application.MovableInstances.Interfaces;

public interface IMovableInstanceService
{
    Task<uint> CreateAsync(uint itemId, uint issuerId, CancellationToken ct = default);
    Task DeleteAsync(uint itemId, uint id, uint issuerId, CancellationToken ct = default);
    Task<byte[]> GetQrCodeAsync(uint itemId, uint instanceId, CancellationToken ct = default);
}
