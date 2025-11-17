using ItTrAp.QueryService.Application.Responses;

namespace ItTrAp.QueryService.Application.Interfaces.Services;
// TODO: use interfaces of collections instead of concrete types

public interface IUserService
{
    Task<List<UserViewModel>> GetUsersByIdsAsync(List<uint> userIds, CancellationToken cancellationToken = default);
    Task<List<UserViewModel>> GetUsersAsync(CancellationToken cancellationToken = default);
}