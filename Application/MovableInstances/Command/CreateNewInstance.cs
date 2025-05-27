using Application.MovableInstances.Interfaces;
using MediatR;

namespace Application.MovableInstances.Command;

public record CreateMovableInstanceCommand(uint ItemId) : IRequest<uint>;

public class CreateMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<CreateMovableInstanceCommand, uint>
{
    public async Task<uint> Handle(CreateMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        return await movableInstanceService.CreateAsync(request.ItemId, cancellationToken);
    }
}