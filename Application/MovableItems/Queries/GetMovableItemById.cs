using Application.Common.ViewModels;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetMovableItemByIdQuery(uint Id) : IRequest<MovableItemViewModel?>;

public class GetMovableItemByIdHandler(IMovableItemReadRepository repo) : IRequestHandler<GetMovableItemByIdQuery, MovableItemViewModel?>
{
    public async Task<MovableItemViewModel?> Handle(GetMovableItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetByIdAsync(request.Id);
    }
}