using Application.Common.DTOs;
using Application.UserSelfManagement.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.UserSelfManagement.Queries;

public record GetUserSelfQuery(uint Id) : IRequest<UserDto?>;

public class GetUserSelfQueryValidator : AbstractValidator<GetUserSelfQuery>
{
    public GetUserSelfQueryValidator()
    {
        RuleFor(x => (int)x.Id).GreaterThan(0).WithMessage("User ID must be greater than 0.");
    }
}

public class GetUserSelfHandler(IUserSelfManagementReadRepository userRepository) : IRequestHandler<GetUserSelfQuery, UserDto?>
{
  public async Task<UserDto?> Handle(GetUserSelfQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdAsync(request.Id);
    }
}