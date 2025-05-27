using Application.Common.ViewModels;
using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Queries;

public record GetMovableInstanceByIdQuery(uint ItemId, uint Id) : IRequest<MovableInstanceViewModel?>;

public class GetMovableInstanceByIdQueryHandler(IMovableInstanceReadRepository repo) : IRequestHandler<GetMovableInstanceByIdQuery, MovableInstanceViewModel?>
{
    public Task<MovableInstanceViewModel?> Handle(GetMovableInstanceByIdQuery request, CancellationToken cancellationToken)
    {
        return repo.GetByIdAsync(request.ItemId, request.Id);
    }
}