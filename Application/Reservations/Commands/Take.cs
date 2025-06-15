using Application.Reservations.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.Reservations.Commands;

public record TakeCommand(uint UserId, uint InstanceId) : IRequest;

public class TakeCommandValidator : AbstractValidator<TakeCommand>
{
    public TakeCommandValidator()
    {
        RuleFor(x => (int)x.InstanceId).NotEqual(0).WithMessage("Instance ID must not be zero.");
        RuleFor(x => (int)x.UserId).NotEqual(0).WithMessage("User ID must not be zero.");
    }
}

public class TakeCommandHandler(IReservationService reservationsService) : IRequestHandler<TakeCommand>
{
    public async Task Handle(TakeCommand request, CancellationToken ct)
    {
        await reservationsService.TakeAsync(request.UserId, request.InstanceId, ct);
    }
}