using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableItems;

public record GetInstanceAmountsByItemIdsQuery(IList<Guid> ItemIds) : IRequest<IList<int>>;

public class GetInstanceAmountsByItemIdsHandler : IRequestHandler<GetInstanceAmountsByItemIdsQuery, IList<int>>
{
    private readonly IMovableInstanceReadRepository _repository;

    public GetInstanceAmountsByItemIdsHandler(IMovableInstanceReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<IList<int>> Handle(GetInstanceAmountsByItemIdsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetInstanceAmountsByItemIdsAsync(request.ItemIds, cancellationToken);
    }
}