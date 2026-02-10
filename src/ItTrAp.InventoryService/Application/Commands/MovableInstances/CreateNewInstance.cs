using ItTrAp.InventoryService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.InventoryService.Application.Commands.MovableInstances;

public record CreateMovableInstanceCommand(Guid ItemId) : IRequest<uint>;

public class CreateMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<CreateMovableInstanceCommand, uint>
{
    public async Task<uint> Handle(CreateMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        return await movableInstanceService.CreateAsync(request.ItemId, cancellationToken);
    }
}