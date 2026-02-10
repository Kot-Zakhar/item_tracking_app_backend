using ItTrAp.ManagementService.Application.DTOs.MovableInstances;
using ItTrAp.ManagementService.Application.Interfaces.Repositories;
using MediatR;

namespace ItTrAp.ManagementService.Application.Queries.MovableInstances;

public record GetMovableInstanceByIdQuery(Guid ItemId, uint Id) : IRequest<MovableInstanceDto?>;

public class GetMovableInstanceByIdQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetMovableInstanceByIdQuery, MovableInstanceDto?>
{
    public Task<MovableInstanceDto?> Handle(GetMovableInstanceByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.ItemId, request.Id);
    }
}