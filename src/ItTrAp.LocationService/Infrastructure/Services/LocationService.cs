using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using ItTrAp.LocationService.Domain.Aggregates;
using ItTrAp.LocationService.Domain.Interfaces;
using ItTrAp.LocationService.Domain.Events.Locations;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces;
using ItTrAp.LocationService.Infrastructure.Persistence.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.LocationService.Infrastructure.Services;

public class LocationService(
    ILocationRepository repo,
    IUnitOfWork unitOfWork,
    Lazy<ILocationUniquenessChecker> nameUniquenessChecker,
    IMediator mediator) : ILocationService
{
    public async Task<uint> CreateLocationAsync(CreateLocationDto createDto, CancellationToken ct = default)
    {
        var location = await Location.CreateAsync(createDto.Name, createDto.Floor, createDto.Department, nameUniquenessChecker.Value, ct);

        location = await repo.CreateAsync(location, ct);

        await unitOfWork.SaveChangesAsync(ct);

        await mediator.Publish(new LocationCreated(location.Id), ct);

        return location.Id;
    }

    public async Task UpdateLocationAsync(uint id, UpdateLocationDto updateData, CancellationToken ct = default)
    {
        var existingLocation = await repo.GetByIdAsync(id, ct);
        if (existingLocation == null)
            throw new ArgumentException($"Location with ID {id} not found.");

        await existingLocation.UpdateAsync(updateData.Name, updateData.Floor, updateData.Department, nameUniquenessChecker.Value, ct);

        await repo.UpdateAsync(existingLocation, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }

    public async Task DeleteLocationAsync(uint id, CancellationToken ct = default)
    {
        var existingLocation = await repo.GetByIdAsync(id, ct);
        if (existingLocation == null)
            throw new ArgumentException($"Location with ID {id} not found.");

        await repo.DeleteAsync(id, ct);
        await unitOfWork.SaveChangesAsync(ct);

        await mediator.Publish(new LocationDeleted(existingLocation.Id), ct);
    }
}
