using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class LocationService(ILocationRepository repo, IUnitOfWork unitOfWork) : ILocationService
{
    public async Task CreateAsync(uint locationId, CancellationToken cancellationToken)
    {
        var location = Location.Create(locationId, Guid.NewGuid());
        await repo.CreateAsync(location, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(uint locationId, CancellationToken cancellationToken)
    {
        var location = await repo.GetByIdAsync(locationId, cancellationToken);
        if (location != null)
        {
            await repo.DeleteAsync(locationId, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}