using Application.Users.DTOs;
using Application.Users.Interfaces;
using MediatR;

namespace Application.Users.Queries;

public record GetAllFilteredUsersQuery(string? Search, int? Top) : IRequest<List<UserWithDetailsDto>>;

public class GetAllFilteredUsersHandler(IUserReadRepository usersRepository) : IRequestHandler<GetAllFilteredUsersQuery, List<UserWithDetailsDto>>
{
    public async Task<List<UserWithDetailsDto>> Handle(GetAllFilteredUsersQuery request, CancellationToken cancellationToken)
    {
        return await usersRepository.GetAllFiltered(request.Search, request.Top);
    }
}
