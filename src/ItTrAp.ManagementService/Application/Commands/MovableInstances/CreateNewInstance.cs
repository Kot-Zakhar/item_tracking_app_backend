using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.MovableInstances;

public record CreateMovableInstanceCommand(Guid ItemId, uint InstanceId) : IRequest;

public class CreateMovableInstanceCommandHandler(IMovableInstanceService movableInstanceService) : IRequestHandler<CreateMovableInstanceCommand>
{
    public async Task Handle(CreateMovableInstanceCommand request, CancellationToken cancellationToken)
    {
        await movableInstanceService.CreateAsync(request.ItemId, request.InstanceId, cancellationToken);
    }
}