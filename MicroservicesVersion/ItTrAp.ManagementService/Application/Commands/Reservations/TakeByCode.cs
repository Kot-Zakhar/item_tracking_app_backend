using ItTrAp.ManagementService.Application.Interfaces.Services;
using FluentValidation;
using MediatR;

namespace ItTrAp.ManagementService.Application.Commands.Reservations;

public record TakeByCodeCommand(uint UserId, Guid Code) : IRequest;

public class TakeByCodeCommandValidator : AbstractValidator<TakeByCodeCommand>
{
    public TakeByCodeCommandValidator()
    {
        RuleFor(x => (int)x.UserId).GreaterThan(0);
        RuleFor(x => x.Code).NotEmpty();
    }
}

public class TakeByCodeCommandHandler(IReservationService reservationsService) : IRequestHandler<TakeByCodeCommand>
{
    public async Task Handle(TakeByCodeCommand request, CancellationToken cancellationToken)
    {
        await reservationsService.TakeByCodeAsync(request.UserId, request.Code, cancellationToken);
    }
}