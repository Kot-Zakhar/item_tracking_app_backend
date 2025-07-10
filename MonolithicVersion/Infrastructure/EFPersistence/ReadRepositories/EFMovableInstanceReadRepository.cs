using Application.Common.DTOs;
using Application.MovableInstances.DTOs;
using Application.MovableInstances.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence.ReadRepositories;

public class EFMovableInstanceReadRepository(AppDbContext context) : IMovableInstanceReadRepository
{
    public Task<List<MovableInstanceDto>> GetAllFilteredAsync(uint itemId, MovableInstanceFiltersDto filters, CancellationToken ct = default)
    {
        var query = context.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.MovableItem.Id == itemId);

        if (filters.Status.HasValue)
        {
            query = query.Where(instance => instance.Status == filters.Status.Value);
        }

        if (filters.LocationId.HasValue)
        {
            query = query.Where(instance => instance.Location != null && instance.Location.Id == filters.LocationId.Value);
        }

        if (filters.UserIds != null && filters.UserIds.Count > 0)
        {
            query = query.Where(instance => instance.User != null && filters.UserIds.Contains(instance.User.Id));
        }

        return query
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                Code = instance.Code,
                Status = instance.Status,
                Location = instance.Location == null ? null : new LocationDto
                {
                    Id = instance.Location.Id,
                    Name = instance.Location.Name,
                    Floor = instance.Location.Floor,
                    Code = instance.Location.Code,
                    Department = instance.Location.Department,
                    CreatedAt = instance.Location.CreatedAt,
                },
                User = instance.User == null ? null : new UserDto
                {
                    Id = instance.User.Id,
                    FirstName = instance.User.FirstName,
                    LastName = instance.User.LastName,
                    Phone = instance.User.Phone,
                    Email = instance.User.Email,
                    Avatar = instance.User.Avatar,
                },
                CreatedAt = instance.CreatedAt,
            })
            .ToListAsync(ct);
    }

    public Task<MovableInstanceDto?> GetByIdAsync(uint itemId, uint id, CancellationToken ct = default)
    {
        return context.MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.MovableItem.Id == itemId && instance.Id == id)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                MovableItemId = instance.MovableItem.Id,
                Code = instance.Code,
                Status = instance.Status,
                Location = instance.Location == null ? null : new LocationDto
                {
                    Id = instance.Location.Id,
                    Name = instance.Location.Name,
                    Floor = instance.Location.Floor,
                    Code = instance.Location.Code,
                    Department = instance.Location.Department,
                    CreatedAt = instance.Location.CreatedAt,
                },
                User = instance.User == null ? null : new UserDto
                {
                    Id = instance.User.Id,
                    FirstName = instance.User.FirstName,
                    LastName = instance.User.LastName,
                    Phone = instance.User.Phone,
                    Email = instance.User.Email,
                    Avatar = instance.User.Avatar,
                },
                CreatedAt = instance.CreatedAt,
            })
            .FirstOrDefaultAsync(ct);
    }
}
