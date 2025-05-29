using Application.Common.DTOs;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Queries;

public record GetMovableItemByIdQuery(uint Id) : IRequest<MovableItemDto?>;

public class GetMovableItemByIdHandler(IMovableItemReadRepository repo) : IRequestHandler<GetMovableItemByIdQuery, MovableItemDto?>
{
    public async Task<MovableItemDto?> Handle(GetMovableItemByIdQuery request, CancellationToken cancellationToken)
    {
        return await repo.GetByIdAsync(request.Id);
    }
}