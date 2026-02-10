using ItTrAp.InventoryService.Application.DTOs.MovableInstances;
using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableInstances;

public record GetMovableInstancesByItemIdQuery(Guid ItemId) : IRequest<IList<MovableInstanceDto>>;

public class GetMovableInstancesByItemIdQueryHandler : IRequestHandler<GetMovableInstancesByItemIdQuery, IList<MovableInstanceDto>>
{
    private readonly IMovableInstanceReadRepository _movableInstanceRepository;

    public GetMovableInstancesByItemIdQueryHandler(IMovableInstanceReadRepository movableInstanceRepository) {
        _movableInstanceRepository = movableInstanceRepository;
    }

    public async Task<IList<MovableInstanceDto>> Handle(GetMovableInstancesByItemIdQuery request, CancellationToken cancellationToken)
    {
        var movableInstances = await _movableInstanceRepository.GetByItemIdAsync(request.ItemId, cancellationToken);
        return movableInstances;
    }
}