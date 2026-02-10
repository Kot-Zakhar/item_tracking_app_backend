using ItTrAp.QueryService.Application.Responses;
using ItTrAp.QueryService.Application.DTOs;

namespace ItTrAp.QueryService.Application.Interfaces.Services;

public interface IInventoryService
{
    Task<IList<MovableItemViewModel>> GetMovableItemsAsync(List<Guid>? itemIds, List<uint>? categoryIds, string? search, CancellationToken cancellationToken = default);
    Task<IList<uint>> GetInstanceAmountsByItemIdsAsync(List<Guid> itemIds, CancellationToken cancellationToken = default);
    Task<IList<MovableInstanceDto>> GetMovableInstancesByItemIdAsync(Guid movableItemId, CancellationToken cancellationToken);
}