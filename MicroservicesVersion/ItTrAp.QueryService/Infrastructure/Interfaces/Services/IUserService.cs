using ItTrAp.QueryService.Application.Responses;

namespace ItTrAp.QueryService.Infrastructure.Interfaces.Services;
// TODO: use interfaces of collections instead of concrete types

public interface IUserService
{
    Task<List<UserViewModel>> GetUsersByIdsAsync(List<uint> userIds, CancellationToken cancellationToken = default);
}