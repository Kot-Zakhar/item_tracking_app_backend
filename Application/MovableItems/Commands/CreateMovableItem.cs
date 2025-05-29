using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Commands;

public record CreateMovableItemCommand(CreateMovableItemDto MovableItem) : IRequest<uint>;

public class CreateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<CreateMovableItemCommand, uint>
{
    public async Task<uint> Handle(CreateMovableItemCommand request, CancellationToken cancellationToken)
    {
        return await movableItemService.CreateAsync(request.MovableItem, cancellationToken);
    }
}
