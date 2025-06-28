using Application.Common.DTOs;
using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Infrastructure.EFPersistence;

// TODO: Use AutoMapper to map joined entities
// TODO: Fix Avatar property in UserViewModel
// TODO: Apply filters to the query

public class EfMovableItemReadRepository : IMovableItemReadRepository, IMovableItemUniquenessChecker
{
    private readonly AppDbContext _dbContext;

    public EfMovableItemReadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<MovableItemWithDetailsDto>> GetAllFilteredWithDetailsAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    )
    {
        var query = _dbContext.MovableItems
              .AsNoTracking();

        if (filters.Status.HasValue)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.Status == filters.Status.Value));
        }
        if (filters.CategoryIds != null && filters.CategoryIds.Count > 0)
        {
            query = query.Where(item => filters.CategoryIds.Contains(item.Category.Id));
        }
        if (filters.LocationId.HasValue)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.Location != null && instance.Location.Id == filters.LocationId.Value));
        }
        if (filters.UserIds != null && filters.UserIds.Count > 0)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.User != null && filters.UserIds.Contains(instance.User.Id)));
        }
        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var search = filters.Search.ToLower();
            query = query.Where(item => item.Name.ToLower().Contains(search));
        }

        return await query
            .AsNoTracking()
            .Select(item => new MovableItemWithDetailsDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
                TotalAmount = item.Instances.Count(),
                BookedBy = item.Instances
                    .Where(instance => instance.Status == MovableInstanceStatus.Booked)
                    .Where(instance => instance.User != null)
                    .Select(instance => new UserDto
                    {
                        Id = instance.User!.Id,
                        FirstName = instance.User.FirstName,
                        LastName = instance.User.LastName,
                        Phone = instance.User.Phone,
                        Email = instance.User.Email,
                        Avatar = instance.User.Avatar
                    })
                    .ToList(),
                TakenBy = item.Instances
                    .Where(instance => instance.Status == MovableInstanceStatus.Taken)
                    .Where(instance => instance.User != null)
                    .Select(instance => new UserDto
                    {
                        Id = instance.User!.Id,
                        FirstName = instance.User.FirstName,
                        LastName = instance.User.LastName,
                        Phone = instance.User.Phone,
                        Email = instance.User.Email,
                        Avatar = instance.User.Avatar
                    })
                    .ToList(),
            })
            .ToListAsync(ct);
    }

    public async Task<List<MovableItemWithAmountsByStatusDto>> GetAllWithAmountPerStatusAsync(
        MovableItemFiltersDto filters,
        CancellationToken ct = default
    )
    {
        var query = _dbContext.MovableItems
              .AsNoTracking();

        if (filters.Status.HasValue)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.Status == filters.Status.Value));
        }
        if (filters.CategoryIds != null && filters.CategoryIds.Count > 0)
        {
            query = query.Where(item => filters.CategoryIds.Contains(item.Category.Id));
        }
        if (filters.LocationId.HasValue)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.Location != null && instance.Location.Id == filters.LocationId.Value));
        }
        if (filters.UserIds != null && filters.UserIds.Count > 0)
        {
            query = query.Where(item => item.Instances.Any(instance => instance.User != null && filters.UserIds.Contains(instance.User.Id)));
        }
        if (!string.IsNullOrWhiteSpace(filters.Search))
        {
            var search = filters.Search.ToLower();
            query = query.Where(item => item.Name.ToLower().Contains(search));
        }

        return await query
            .Select(item => new MovableItemWithAmountsByStatusDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
                AmountsByStatus = item.Instances
                    .GroupBy(instance => instance.Status)
                    .Select(group => new
                    {
                        Status = group.Key,
                        Amount = group.Count()
                    })
                    .ToDictionary(group => group.Status, group => group.Amount),
            })
            .ToListAsync(ct);
    }

    public Task<MovableItemDto?> GetByIdAsync(uint id, CancellationToken ct = default)
    {
        return _dbContext.MovableItems
            .AsNoTracking()
            .Include(item => item.Category)
            .Where(item => item.Id == id)
            .Select(item => new MovableItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Category = new CategoryDto
                {
                    Id = item.Category.Id,
                    Name = item.Category.Name,
                    Icon = item.Category.Icon
                },
                Visibility = item.Visibility,
                CreatedAt = item.CreatedAt,
                ImgSrc = item.ImgSrc,
            })
            .FirstOrDefaultAsync(ct);
    }

    public Task<bool> IsUniqueAsync(string name, CancellationToken ct = default)
        => _dbContext.MovableItems.AllAsync(i => i.Name != name, ct);

    public Task<bool> IsUniqueAsync(uint id, string name, CancellationToken ct = default)
        => _dbContext.MovableItems.Where(x => x.Id != id).AllAsync(x => x.Name != name, ct);
}