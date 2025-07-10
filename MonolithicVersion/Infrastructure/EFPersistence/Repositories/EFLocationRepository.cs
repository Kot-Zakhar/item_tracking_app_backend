using Domain.Aggregates.Locations;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Interfaces.Persistence.Repositories;

namespace Infrastructure.EFPersistence.Repositories;

public class EFLocationRepository(AppDbContext context) : EFRepository<Location>(context), ILocationRepository
{
    private readonly AppDbContext context = context;
  
    public async Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return await context.Locations.FirstOrDefaultAsync(l => l.Code == code, ct);
    }
}