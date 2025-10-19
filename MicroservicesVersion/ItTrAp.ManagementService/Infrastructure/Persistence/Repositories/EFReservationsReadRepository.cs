using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using ItTrAp.ManagementService.Application.DTOs.Reservations;

namespace ItTrAp.ManagementService.Infrastructure.Persistence.Repositories;

public class EFReservationsReadRepository(AppDbContext dbContext) : IReservationReadRepository
{
    public Task<List<MovableInstanceDto>> GetAssociatedItemInstancesAsync(uint userId, CancellationToken cancellationToken)
    {
        return dbContext
            .MovableInstances
            .AsNoTracking()
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

    public async Task<IList<InstanceStatusDto>> GetInstanceStatusesByItemIdAsync(Guid itemId, CancellationToken cancellationToken)
    {
        return await dbContext
            .MovableInstances
            .AsNoTracking()
            .Where(instance => instance.MovableItem.Id == itemId)
            .Select(instance => new InstanceStatusDto
            {
                Id = instance.Id,
                ItemId = instance.MovableItem.Id,
                Status = instance.Status,
                UserId = instance.User != null ? instance.User.Id : null,
                LocationId = instance.Location != null ? instance.Location.Id : null
            })
            .ToListAsync(cancellationToken);   
    }

    public async Task<Dictionary<Guid, List<UserStatusDto>>> GetUserStatusesForItemsAsync(List<Guid> itemIds, CancellationToken cancellationToken)
    {
        var query = dbContext
            .MovableInstances
            .AsNoTracking()
            .Where(instance => instance.User != null && itemIds.Contains(instance.MovableItem.Id));

        var statuses = await query
            .Select(instance => new
            {
                UserId = instance.User!.Id,
                MovableItemId = instance.MovableItem.Id,
                instance.Status
            })
            .ToListAsync(cancellationToken);

        return statuses.GroupBy(status => status.MovableItemId)
            .ToDictionary(group => group.Key, group => group.Select(g => new UserStatusDto
            {
                UserId = g.UserId,
                Status = g.Status
            }).ToList());
    }
    
    public async Task<IList<uint>> GetItemAmountsByUserIdsAsync(IList<uint> userIds, CancellationToken cancellationToken)
    {
        var amounts = await dbContext
            .MovableInstances
            .AsNoTracking()
            .Where(instance => instance.User != null && userIds.Contains(instance.User.Id))
            .GroupBy(instance => instance.User!.Id)
            .Select(group => new
            {
                UserId = group.Key,
                Amount = group.Count()
            })
            .ToListAsync(cancellationToken);

        var amountsDict = amounts.ToDictionary(a => a.UserId, a => (uint)a.Amount);

        var result = userIds.Select(userId => amountsDict.ContainsKey(userId) ? amountsDict[userId] : 0).ToList();

        return result;
    }
}