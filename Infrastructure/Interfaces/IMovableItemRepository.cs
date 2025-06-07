using Abstractions;
using Domain.Models;

namespace Infrastructure.Interfaces;

public interface IMovableInstanceRepository : IRepository<MovableInstance>
{
    Task<MovableInstance?> GetByCodeAsync(Guid code, CancellationToken ct = default);
}