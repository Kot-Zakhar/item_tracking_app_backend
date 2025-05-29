using Application.MovableItems.DTOs;
using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Commands;

public record UpdateMovableItemCommand(uint Id, UpdateMovableItemDto MovableItem) : IRequest;

public class UpdateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<UpdateMovableItemCommand>
{
    public async Task Handle(UpdateMovableItemCommand request, CancellationToken cancellationToken)
    {
        await movableItemService.UpdateAsync(request.Id, request.MovableItem);
    }
}