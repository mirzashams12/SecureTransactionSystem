using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User> AddAsync(User user);
    Task<User> GetByEmailAsync(string email);
    Task<bool> DeleteAsync(Guid id);
    Task<User> GetByIdAsync(Guid id);
    Task<User> UpdateAsync(User user);
}