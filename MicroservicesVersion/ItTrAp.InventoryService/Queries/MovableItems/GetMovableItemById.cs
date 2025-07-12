using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Queries.MovableItems;

public record GetMovableItemByIdQuery(uint Id) : IRequest<MovableItemDto?>;

public class GetMovableItemByIdHandler(IMovableItemReadRepository repo) : IRequestHandler<GetMovableItemByIdQuery, MovableItemDto?>
{
    public async Task<MovableItemDto?> Handle(GetMovableItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetByIdAsync(request.Id);
    }
}