using Application.MovableItems.Dtos;
using Application.MovableItems.Interfaces;
using Application.MovableItems.ViewModels;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetAllFilteredMovableItemsQuery (MovableItemFiltersDto filters)
    : IRequest<List<MovableItemWithDetailsViewModel>>;

public class GetAllFilteredMovableItemsWithDetailsHandler(IMovableItemReadRepository movableItemReadRepository)
    : IRequestHandler<GetAllFilteredMovableItemsQuery, List<MovableItemWithDetailsViewModel>>
{
    public async Task<List<MovableItemWithDetailsViewModel>> Handle(GetAllFilteredMovableItemsQuery request, CancellationToken cancellationToken)
    {
        return await movableItemReadRepository.GetAllFilteredWithDetailsAsync(request.filters, cancellationToken);
    }
}
