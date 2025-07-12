using ItTrAp.InventoryService.Interfaces.Services;
using MediatR;

namespace ItTrAp.Commands.MovableItems;

public record DeleteMovableItemCommand(uint Id) : IRequest;

public class DeleteMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<DeleteMovableItemCommand>
{
    public async Task Handle(DeleteMovableItemCommand request, CancellationToken cancellationToken)
    {
        await movableItemService.DeleteAsync(request.Id, cancellationToken);
    }
}