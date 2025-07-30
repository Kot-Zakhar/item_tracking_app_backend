namespace ItTrAp.ManagementService.Application.Interfaces.Services;

public interface IUserService
{
    Task CreateAsync(uint userId, string userEmail, CancellationToken cancellationToken);
    Task DeleteAsync(uint userId, CancellationToken cancellationToken);
}
