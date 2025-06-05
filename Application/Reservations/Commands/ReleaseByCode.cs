using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record ReleaseByCodeCommand(uint UserId, Guid InstanceCode, Guid LocationCode) : IRequest;

public class ReleaseByCodeCommandValidator : AbstractValidator<ReleaseByCodeCommand>
{
    public ReleaseByCodeCommandValidator()
    {
        RuleFor(x => (int)x.UserId).NotEqual(0).WithMessage("Instance ID must not be zero.");
        RuleFor(x => x.InstanceCode).NotEqual(Guid.Empty).WithMessage("User ID must not be empty.");
        RuleFor(x => x.LocationCode).NotEqual(Guid.Empty).WithMessage("Location ID must not be empty.");
    }
}

public class ReleaseByCodeCommandHandler(IReservationService reservationsService) : IRequestHandler<ReleaseByCodeCommand>
{
    public async Task Handle(ReleaseByCodeCommand request, CancellationToken cancellationToken)
    {
        await reservationsService.ReleaseAsync(request.UserId, request.InstanceCode, request.LocationCode, cancellationToken);
    }
}