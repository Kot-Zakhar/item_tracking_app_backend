using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record ReleaseCommand(uint UserId, uint InstanceId, uint LocationId) : IRequest;

public class ReleaseCommandValidator : AbstractValidator<ReleaseCommand>
{
    public ReleaseCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).GreaterThan(0).WithMessage("Instance ID must be greater than 0.");
        RuleFor(x => (int)x.LocationId).GreaterThan(0).WithMessage("Location ID must be greater than 0.");
        RuleFor(x => (int)x.UserId).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class ReleaseCommandHandler(IReservationService reservationService) : IRequestHandler<ReleaseCommand>
{
    public async Task Handle(ReleaseCommand request, CancellationToken cancellationToken)
    {
        await reservationService.ReleaseForcefullyAsync(request.UserId, request.InstanceId, request.LocationId, cancellationToken);
    }
}