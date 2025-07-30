using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.MovableInstances;

public record CreateMovableInstanceCommand(Guid ItemId, uint IssuerId) : IRequest<uint>;

public class CreateMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<CreateMovableInstanceCommand, uint>
{
    public async Task<uint> Handle(CreateMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        return await movableInstanceService.CreateAsync(request.ItemId, request.IssuerId, cancellationToken);
    }
}