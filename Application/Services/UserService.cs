using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IGuidService _guidService;
    private readonly IUserRepository _repo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDataService _dateService;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;

    public UserService(IGuidService guidService,
                        IUserRepository repo,
                        IPasswordHasher passwordHasher,
                        IDataService dateService,
                        IMapper mapper,
                        ITokenService tokenService)
    {
        _guidService = guidService;
        _repo = repo;
        _passwordHasher = passwordHasher;
        _dateService = dateService;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        List<User> users = await _repo.GetAllAsync();
        return _mapper.Map<List<UserDto>>(users);
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto userDto)
    {
        User user = _mapper.Map<User>(userDto);
        user.Id = _guidService.GetGuid();
        user.PasswordHash = _passwordHasher.HashPassword(userDto.Password);
        user.CreatedAt = _dateService.GetCurrentDate();
        user.UpdatedAt = _dateService.GetCurrentDate();

        User result = await _repo.AddAsync(user);
        return _mapper.Map<UserDto>(result);
    }

    public Task<TokenDto> AuthenticateUserAsync(UserAuthenticateDto userDto)
    {
        try
        {
            User user = _repo.GetByEmailAsync(userDto.Email).Result;

            if (_passwordHasher.VerifyPassword(user.PasswordHash, userDto.Password))
            {
                TokenDto result = _tokenService.GenerateToken(user.Id, user.Email);
                return Task.FromResult(result);
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid user.");
            }
        }
        catch (AggregateException ex) when (ex.InnerException is InvalidOperationException)
        {
            throw new UnauthorizedAccessException("Invalid user.");
        }
    }

    public async Task<bool> DeleteUserAsync(Guid id)
    {
        return await _repo.DeleteAsync(id);
    }

    public async Task<UserDto> GetUserByIdAsync(Guid id)
    {
        User user = await _repo.GetByIdAsync(id);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        User user = await _repo.GetByEmailAsync(email);
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> UpdateUserAsync(Guid id, string name, string email)
    {
        var existingUser = await _repo.GetByIdAsync(id);
        existingUser.Name = name;
        existingUser.Email = email;
        existingUser.UpdatedAt = _dateService.GetCurrentDate();

        User updatedUser = await _repo.UpdateAsync(existingUser);
        return _mapper.Map<UserDto>(updatedUser);
    }

}