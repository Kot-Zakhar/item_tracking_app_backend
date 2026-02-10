using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Infrastructure.Interfaces.Persistence.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableItems;

public record GetAllFilteredMovableItemsQuery (MovableItemFiltersDto filters)
    : IRequest<List<MovableItemDto>>;

public class GetAllFilteredMovableItemsHandler(IMovableItemReadRepository movableItemReadRepository)
    : IRequestHandler<GetAllFilteredMovableItemsQuery, List<MovableItemDto>>
{
    public async Task<List<MovableItemDto>> Handle(GetAllFilteredMovableItemsQuery request, CancellationToken cancellationToken)
    {
        return await movableItemReadRepository.GetAllFilteredAsync(request.filters, cancellationToken);
    }
}
