namespace Application.Users.Interfaces;

using Application.Common.ViewModels;

public interface IUserReadRepository
{
    Task<UserViewModel?> GetByIdAsync(uint id, CancellationToken ct = default);
    Task<List<UserViewModel>> GetAllFiltered(string? search, int? top, CancellationToken ct = default);
}