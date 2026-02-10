using ItTrAp.ManagementService.Application.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Reservations;

public record ReleaseCommand(uint IssuerId, uint InstanceId, uint LocationId) : IRequest;

public class ReleaseCommandValidator : AbstractValidator<ReleaseCommand>
{
    public ReleaseCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).GreaterThan(0).WithMessage("Instance ID must be greater than 0.");
        RuleFor(x => (int)x.LocationId).GreaterThan(0).WithMessage("Location ID must be greater than 0.");
        RuleFor(x => (int)x.IssuerId).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class ReleaseCommandHandler(IReservationService reservationService) : IRequestHandler<ReleaseCommand>
{
    public async Task Handle(ReleaseCommand request, CancellationToken cancellationToken)
    {
        await reservationService.ReleaseForcefullyAsync(request.IssuerId, request.InstanceId, request.LocationId, cancellationToken);
    }
}