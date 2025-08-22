using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFReservationsReadRepository(AppDbContext dbContext) : IReservationReadRepository
{
    public Task<List<MovableInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken)
    {
        return dbContext
            .MovableInstances
            .AsNoTracking()
            .Include(instance => instance.MovableItem)
            .Include(instance => instance.Location)
            .Include(instance => instance.User)
            .Where(instance => instance.Status == MovableInstanceStatus.Booked ||
                               instance.Status == MovableInstanceStatus.Taken)
            .Where(instance => instance.User != null && instance.User.Id == userId)
            .Select(instance => new MovableInstanceDto
            {
                Id = instance.Id,
                Code = instance.Code,
                Status = instance.Status,
                LocationId = instance.Location != null ? instance.Location.Id : null,
                UserId = instance.User != null ? instance.User.Id : null,
                MovableItemId = instance.MovableItem.Id,
            })
            .ToListAsync(cancellationToken);
        
    }
}