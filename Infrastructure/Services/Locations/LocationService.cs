using Application.Locations.Dtos;
using Application.Locations.Interfaces;
using Domain.Locations;
using Domain.Locations.Interfaces;
using Infrastructure.Interfaces.Common;
using Infrastructure.Interfaces.Locations;

namespace Infrastructure.Services.Locations;

public class LocationService(ILocationRepository repo, IUnitOfWork unitOfWork, Lazy<ILocationUniquenessChecker> nameUniquenessChecker) : ILocationService
{
    public async Task<uint> CreateLocationAsync(CreateLocationDto createDto, CancellationToken ct = default)
    {
        var location = await Location.CreateAsync(createDto.Name, createDto.Floor, createDto.Department, nameUniquenessChecker.Value, ct);

        location = await repo.CreateAsync(location, ct);

        await unitOfWork.SaveChangesAsync(ct);

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
        await repo.DeleteAsync(id, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
