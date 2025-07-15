using ItTrAp.InventoryService.Application.DTOs.MovableItems;
using ItTrAp.InventoryService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.Commands.MovableItems;

public record UpdateMovableItemCommand(Guid Id, UpdateMovableItemDto MovableItem) : IRequest;

public class UpdateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<UpdateMovableItemCommand>
{
    public async Task Handle(UpdateMovableItemCommand request, CancellationToken cancellationToken)
    {
        await movableItemService.UpdateAsync(request.Id, request.MovableItem);
    }
}