using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Infrastructure.DTOs;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;

public interface IInventoryService
{
    Task<IList<MovableItemViewModel>> GetMovableItemsAsync(List<uint>? categoryIds, string? search, CancellationToken cancellationToken = default);
    Task<IList<uint>> GetInstanceAmountsByItemIdsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default);
    Task<IList<MovableInstanceDto>> GetMovableInstancesByItemIdAsync(Guid movableItemId, CancellationToken cancellationToken);
}