using ItTrAp.ManagementService.Application.Interfaces.Services;
using ItTrAp.ManagementService.Domain.Aggregates;
using ItTrAp.ManagementService.Infrastructure.Interfaces;
using ItTrAp.ManagementService.Infrastructure.Interfaces.Persistence.Repositories;

namespace ItTrAp.ManagementService.Infrastructure.Services;

public class UserService(IUserRepository repo, IUnitOfWork unitOfWork) : IUserService
{
    public async Task CreateAsync(uint userId, string userEmail, CancellationToken cancellationToken)
    {
        var existingUser = repo.GetAllAsync(cancellationToken)
            .Where(u => u.Email == userEmail || u.Id == userId)
            .FirstOrDefault();

        if (existingUser != null)
        {
            return;
        }

        var user = User.Create(userId, userEmail);
        await repo.CreateAsync(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(uint userId, CancellationToken cancellationToken)
    {
        var user = await repo.GetByIdAsync(userId);

        if (user == null)
        {
            return;
        }

        await repo.DeleteAsync(userId, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}