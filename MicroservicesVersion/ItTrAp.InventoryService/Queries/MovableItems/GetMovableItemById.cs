using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Services;
using MediatR;

namespace ItTrAp.InventoryService.Queries.MovableItems;

public record GetMovableItemByIdQuery(Guid Id) : IRequest<MovableItemWithCategoryDto?>;

public class GetMovableItemByIdHandler(IMovableItemService service) : IRequestHandler<GetMovableItemByIdQuery, MovableItemWithCategoryDto?>
{
    public async Task<MovableItemWithCategoryDto?> Handle(GetMovableItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await service.GetByIdAsync(request.Id);
    }
}