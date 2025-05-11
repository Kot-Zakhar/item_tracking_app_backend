namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(uint id);
    Task<User?> GetByEmailAsync(string email);
	Task<List<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task<bool> DeleteAsync(uint id);
}
