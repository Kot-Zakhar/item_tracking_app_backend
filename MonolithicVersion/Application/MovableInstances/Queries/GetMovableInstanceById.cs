using Application.Common.DTOs;
using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Queries;

public record GetMovableInstanceByIdQuery(uint ItemId, uint Id) : IRequest<MovableInstanceDto?>;

public class GetMovableInstanceByIdQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetMovableInstanceByIdQuery, MovableInstanceDto?>
{
    public Task<MovableInstanceDto?> Handle(GetMovableInstanceByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.ItemId, request.Id);
    }
}