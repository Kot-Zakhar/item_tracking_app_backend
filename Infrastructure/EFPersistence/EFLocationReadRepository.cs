using Application.Common.DTOs;
using Application.Locations.DTOs;
using Application.Locations.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EFLocationReadRepository(AppDbContext dbContext) : ILocationReadRepository, ILocationUniquenessChecker
{
    public Task<List<LocationWithDetailsDto>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default)
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

        if (filters.WithItemsOnly == true)
        {
            query = query.Where(l => l.Instances.Any());
        }

        if (filters.Top != null)
        {
            query = query.OrderBy(l => l.Floor).ThenBy(l => l.Department).ThenBy(l => l.Name).Take(filters.Top.Value);
        }

        return query
            .Select(l => new LocationWithDetailsDto
            {
                Id = l.Id,
                Name = l.Name,
                Floor = l.Floor,
                Department = l.Department,
                Code = l.Code,
                CreatedAt = l.CreatedAt,
                ItemsAmount = l.Instances.Count(),
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
                Code = l.Code,
                CreatedAt = l.CreatedAt
            })
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
    {
        return dbContext.Locations.AllAsync(l => l.Name != name, ct);
    }
}