
using ItTrAp.ManagementService.Application.Interfaces.Services;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Locations;

public record CreateLocationCommand(uint LocationId) : IRequest;

public class CreateLocationCommandHandler(ILocationService locationService) : IRequestHandler<CreateLocationCommand>
{
    public async Task Handle(CreateLocationCommand request, CancellationToken cancellationToken)
    {
        await locationService.CreateAsync(request.LocationId, cancellationToken);
    }
}