using ItTrAp.InventoryService.Application.DTOs.MovableInstances;
using ItTrAp.InventoryService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.InventoryService.Application.Queries.MovableInstances;

public record GetMovableInstanceByIdQuery(Guid ItemId, uint Id) : IRequest<MovableInstanceDto?>;

public class GetMovableInstanceByIdQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetMovableInstanceByIdQuery, MovableInstanceDto?>
{
    public Task<MovableInstanceDto?> Handle(GetMovableInstanceByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.ItemId, request.Id);
    }
}