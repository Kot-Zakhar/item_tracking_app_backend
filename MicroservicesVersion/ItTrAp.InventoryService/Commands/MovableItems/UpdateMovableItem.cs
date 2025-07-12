using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Services;
using MediatR;

namespace ItTrAp.Commands.MovableItems;

public record UpdateMovableItemCommand(uint Id, UpdateMovableItemDto MovableItem) : IRequest;

public class UpdateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<UpdateMovableItemCommand>
{
    public async Task Handle(UpdateMovableItemCommand request, CancellationToken cancellationToken)
    {
        await movableItemService.UpdateAsync(request.Id, request.MovableItem);
    }
}