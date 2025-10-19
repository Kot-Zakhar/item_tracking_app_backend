using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using MediatR;

namespace ItTrAp.UserService.Application.Queries;

public record GetUsersQuery : IRequest<IList<UserDto>>;

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IList<UserDto>>
{
    private readonly IUserReadRepository _userRepository;

    public GetUsersQueryHandler(IUserReadRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IList<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }
}