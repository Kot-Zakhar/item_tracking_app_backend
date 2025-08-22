using ItTrAp.InventoryService.Application.DTOs.MovableInstances;
using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableInstances;

public record GetAllMovableInstancesQuery(Guid ItemId)
    : IRequest<List<MovableInstanceDto>>;

public class GetAllMovableInstancesQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetAllMovableInstancesQuery, List<MovableInstanceDto>>
{
    public async Task<List<MovableInstanceDto>> Handle(GetAllMovableInstancesQuery request,
        CancellationToken cancellationToken)
    {
        return await repo.GetAllAsync(request.ItemId);
    }
}