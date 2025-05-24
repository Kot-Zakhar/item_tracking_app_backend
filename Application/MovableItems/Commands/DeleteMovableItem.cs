using Application.MovableItems.Interfaces;
using MediatR;

namespace Application.MovableItems.Commands;

public record DeleteMovableItemCommand(uint Id) : IRequest;

public class DeleteMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<DeleteMovableItemCommand>
{
    public async Task Handle(DeleteMovableItemCommand request, CancellationToken cancellationToken)
    {
        await movableItemService.DeleteAsync(request.Id, cancellationToken);
    }
}