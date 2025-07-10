using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetAllFilteredMovableItemsWithAmountsByStatusQuery (MovableItemFiltersDto filters)
    : IRequest<List<MovableItemWithAmountsByStatusDto>>;

public class GetAllFilteredMovableItemsWithAmountsByStatusHandler(IMovableItemReadRepository movableItemReadRepository)
    : IRequestHandler<GetAllFilteredMovableItemsWithAmountsByStatusQuery, List<MovableItemWithAmountsByStatusDto>>
{
    public async Task<List<MovableItemWithAmountsByStatusDto>> Handle(GetAllFilteredMovableItemsWithAmountsByStatusQuery request, CancellationToken cancellationToken)
    {
        return await movableItemReadRepository.GetAllWithAmountPerStatusAsync(request.filters, cancellationToken);
    }
}
