using Application.DTOs;

namespace Application.Interfaces;

public interface IUserService
{
    Task<List<UserDto>> GetAllUsersAsync();
    Task<UserDto> CreateUserAsync(UserCreateDto user);
    Task<TokenDto> AuthenticateUserAsync(UserAuthenticateDto user);
    Task<bool> DeleteUserAsync(Guid id);
    Task<UserDto> GetUserByIdAsync(Guid id);
    Task<UserDto> GetUserByEmailAsync(string email);
    Task<UserDto> UpdateUserAsync(Guid id, string name, string email);
}