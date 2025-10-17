using ItTrAp.UserService.Application.DTOs;
using ItTrAp.UserService.Application.Interfaces;
using MediatR;

namespace ItTrAp.UserService.Application.Queries;

public record GetUsersByIdsQuery(List<uint> UserIds) : IRequest<List<UserDto>>;

public class GetUsersByIdsHandler(IUserReadRepository userRepository) : IRequestHandler<GetUsersByIdsQuery, List<UserDto>>
{
    public async Task<List<UserDto>> Handle(GetUsersByIdsQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetByIdsAsync(request.UserIds, cancellationToken);
    }
}