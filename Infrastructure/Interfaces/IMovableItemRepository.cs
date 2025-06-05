using Abstractions;
using Domain.MovableItems;

namespace Infrastructure.Interfaces;

public interface IMovableInstanceRepository : IRepository<MovableInstance>
{
    Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}