using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFLocationRepository(AppDbContext dbContext) : EFRepository<Location, uint>(dbContext), ILocationRepository
{
    private readonly AppDbContext dbContext = dbContext;
    public Task<Location?> GetByCodeAsync(Guid code, CancellationToken ct = default)
    {
        return dbContext.Locations.FirstOrDefaultAsync(l => l.Code == code, ct);
    }
}
