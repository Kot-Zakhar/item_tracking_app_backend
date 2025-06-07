using Application.Common.DTOs;
using Application.Reservations.DTOs;
using Application.Reservations.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EFPersistence;

public class EFReservationsReadRepository(AppDbContext dbContext) : IReservationReadRepository
{
    public Task<List<ItemInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken)
    {
        return dbContext
            .MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .ThenInclude(item => item.Category)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.Status == Domain.Models.MovableInstanceStatus.Booked ||
                               instance.Status == Domain.Models.MovableInstanceStatus.Taken)
            .Where(instance => instance.User != null && instance.User.Id == userId)
            .Select(instance => new ItemInstanceDto
            {
                Item = new MovableItemDto
                {
                    Id = instance.MovableItem.Id,
                    Name = instance.MovableItem.Name,
                    Description = instance.MovableItem.Description,
                    Category = new CategoryDto
                    {
                        Id = instance.MovableItem.Category.Id,
                        Name = instance.MovableItem.Category.Name,
                        Icon = instance.MovableItem.Category.Icon,
                    },
                    Visibility = instance.MovableItem.Visibility,
                    CreatedAt = instance.MovableItem.CreatedAt,
                    ImgSrc = instance.MovableItem.ImgSrc,
                },
                Instance = new MovableInstanceDto
                {
                    Id = instance.Id,
                    Code = instance.Code,
                    Status = instance.Status,
                    Location = instance.Location == null ? null : new LocationDto
                    {
                        Id = instance.Location.Id,
                        Code = instance.Location.Code,
                        Floor = instance.Location.Floor,
                        Name = instance.Location.Name,
                        Department = instance.Location.Department,
                        CreatedAt = instance.Location.CreatedAt,
                    },
                    CreatedAt = instance.CreatedAt,
                }
            })
            .ToListAsync(cancellationToken);
        
    }
}