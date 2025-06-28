using Application.UserSelfManagement.DTOs;
using Application.UserSelfManagement.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.UserSelfManagement.Commands;

public record UpdateUserSelfCommand(uint Id, UpdateUserSelfDto User) : IRequest;

public class UpdateUserSelfCommandValidator : AbstractValidator<UpdateUserSelfCommand>
{
    public UpdateUserSelfCommandValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
        RuleFor(x => x.User.Phone)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone must be in a valid international format.");
    }
}

public class UpdateUserSelfHandler(IUserSelfManagementService userService) : IRequestHandler<UpdateUserSelfCommand>
{
    public async Task Handle(UpdateUserSelfCommand request, CancellationToken cancellationToken)
    {
        await userService.UpdateUserSelfAsync(request.Id, request.User);
    }
}