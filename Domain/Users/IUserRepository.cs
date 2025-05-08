namespace Domain.Users;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(uint id);
	Task<List<User>> GetAllAsync();
    Task<uint> CreateAsync(User user);
    Task<bool> UpdateAsync(User user);
    Task<bool> DeleteAsync(uint id);
}
