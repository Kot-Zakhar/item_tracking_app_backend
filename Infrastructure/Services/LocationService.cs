using Application.Locations.DTOs;
using Application.Locations.Interfaces;
using Domain.Aggregates.Locations;
using Domain.Interfaces;
using Infrastructure.Interfaces.Persistence;
using Infrastructure.Interfaces.Persistence.Repositories;
using Infrastructure.Interfaces.Services;
using Infrastructure.Models;

namespace Infrastructure.Services;

public class LocationService(
    ILocationRepository repo,
    IUnitOfWork unitOfWork,
    Lazy<ILocationUniquenessChecker> nameUniquenessChecker,
    IQrService qrService) : ILocationService
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

    public async Task<byte[]> GetQrCodeAsync(uint Id, CancellationToken ct = default)
    {
        var location = await repo.GetByIdAsync(Id, ct);
        if (location == null)
        {
            throw new ArgumentException($"Location with ID {Id} does not exist.", nameof(Id));
        }

        return qrService.GetQrCode(QrCodeEntity.Location, location.Code);
    }

}
