using Application.MovableItems.Dtos;
using Application.MovableItems.Interfaces;
using Application.MovableItems.ViewModels;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetAllFilteredMovableItemsWithAmountsByStatusQuery (MovableItemFiltersDto filters)
    : IRequest<List<MovableItemWithAmountsByStatusViewModel>>;

public class GetAllFilteredMovableItemsWithAmountsByStatusHandler(IMovableItemReadRepository movableItemReadRepository)
    : IRequestHandler<GetAllFilteredMovableItemsWithAmountsByStatusQuery, List<MovableItemWithAmountsByStatusViewModel>>
{
    public async Task<List<MovableItemWithAmountsByStatusViewModel>> Handle(GetAllFilteredMovableItemsWithAmountsByStatusQuery request, CancellationToken cancellationToken)
    {
        return await movableItemReadRepository.GetAllWithAmountPerStatusAsync(request.filters, cancellationToken);
    }
}
