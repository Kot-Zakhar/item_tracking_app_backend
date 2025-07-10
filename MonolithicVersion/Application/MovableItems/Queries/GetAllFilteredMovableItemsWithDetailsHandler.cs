using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetAllFilteredMovableItemsQuery (MovableItemFiltersDto filters)
    : IRequest<List<MovableItemWithDetailsDto>>;

public class GetAllFilteredMovableItemsWithDetailsHandler(IMovableItemReadRepository movableItemReadRepository)
    : IRequestHandler<GetAllFilteredMovableItemsQuery, List<MovableItemWithDetailsDto>>
{
    public async Task<List<MovableItemWithDetailsDto>> Handle(GetAllFilteredMovableItemsQuery request, CancellationToken cancellationToken)
    {
        return await movableItemReadRepository.GetAllFilteredWithDetailsAsync(request.filters, cancellationToken);
    }
}
