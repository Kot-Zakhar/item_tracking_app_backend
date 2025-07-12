using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Services;
using MediatR;

namespace ItTrAp.Commands.MovableItems;

public record CreateMovableItemCommand(CreateMovableItemDto MovableItem) : IRequest<uint>;

public class CreateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<CreateMovableItemCommand, uint>
{
    public async Task<uint> Handle(CreateMovableItemCommand request, CancellationToken cancellationToken)
    {
        return await movableItemService.CreateAsync(request.MovableItem, cancellationToken);
    }
}
