using Application.Common.ViewModels;
using Application.Locations.Dtos;
using Application.Locations.Interfaces;
using Application.Locations.ViewModels;
using Domain.Locations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Locations;

public class EFLocationReadRepository(AppDbContext dbContext) : ILocationReadRepository, ILocationUniquenessChecker
{
    public Task<List<LocationWithDetailsViewModel>> GetAllFilteredAsync(LocationFiltersDto filters, CancellationToken ct = default)
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
            query = query.OrderBy(l => new { l.Floor, l.Department, l.Name }).Take(filters.Top.Value);
        }

        return query
            .Select(l => new LocationWithDetailsViewModel
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

    public Task<LocationViewModel?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return dbContext.Locations
            .Where(l => l.Id == id)
            .Select(l => new LocationViewModel
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