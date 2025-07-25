using Microsoft.EntityFrameworkCore;
using ItTrAp.LocationService.Domain.Aggregates;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;

namespace ItTrAp.LocationService.Infrastructure.Persistence.Repositories;

public class EFLocationRepository(AppDbContext context) : EFRepository<Location, uint>(context), ILocationRepository
{
    private readonly AppDbContext context = context;
  
    public async Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return await context.Locations.FirstOrDefaultAsync(l => l.Code == code, ct);
    }
}