using ItTrAp.InventoryService.DTOs.MovableItems;
using ItTrAp.InventoryService.Interfaces.Services;
using MediatR;

namespace ItTrAp.Commands.MovableItems;

public record CreateMovableItemCommand(CreateMovableItemDto MovableItem) : IRequest<Guid>;

public class CreateMovableItemCommandHandler(IMovableItemService movableItemService) : IRequestHandler<CreateMovableItemCommand, Guid>
{
    public async Task<Guid> Handle(CreateMovableItemCommand request, CancellationToken cancellationToken)
    {
        return await movableItemService.CreateAsync(request.MovableItem, cancellationToken);
    }
}
