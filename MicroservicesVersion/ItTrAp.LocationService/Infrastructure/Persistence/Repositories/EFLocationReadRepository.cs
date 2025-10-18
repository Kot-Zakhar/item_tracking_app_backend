using ItTrAp.LocationService.Application.DTOs;
using ItTrAp.LocationService.Application.Interfaces;
using ItTrAp.LocationService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.LocationService.Infrastructure.Persistence.Repositories;

public class EFLocationReadRepository(AppDbContext dbContext) : ILocationReadRepository, ILocationUniquenessChecker
{
    public Task<List<LocationDto>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default)
    {
        var query = dbContext.Locations.AsQueryable();

        if (filters.Floor != null)
        {
            query = query.Where(l => l.Floor == filters.Floor);
        }

        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var search = filters.Search.ToLower();
            query = query.Where(l => ("floor " + l.Floor + ", " + l.Name + ", " + (l.Department ?? "")).ToLower().Contains(search));
        }

        if (filters.Top != null)
        {
            query = query.OrderBy(l => l.Floor).ThenBy(l => l.Department).ThenBy(l => l.Name).Take(filters.Top.Value);
        }

        return query
            .Select(l => new LocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Floor = l.Floor,
                Department = l.Department,
                CreatedAt = l.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public Task<LocationDto?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return dbContext.Locations
            .Where(l => l.Id == id)
            .Select(l => new LocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Floor = l.Floor,
                Department = l.Department,
                CreatedAt = l.CreatedAt
            })
            .FirstOrDefaultAsync(ct);
    }

    public async Task<IList<LocationDto>> GetLocationsByIdsAsync(IList<uint> ids, CancellationToken cancellationToken)
    {
        var locations = await dbContext.Locations
            .Where(l => ids.Contains(l.Id))
            .Select(l => new LocationDto
            {
                Id = l.Id,
                Name = l.Name,
                Floor = l.Floor,
                Department = l.Department,
                CreatedAt = l.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return locations;
    }
    

    public Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
    {
        return dbContext.Locations.AllAsync(l => l.Name != name, ct);
    }

    public Task<bool> IsUniqueAsync(uint id, string name, CancellationToken ct = default)
        => dbContext.Locations.Where(x => x.Id != id).AllAsync(x => x.Name != name, ct);
}