using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public UsersController(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAllUsers()
    {
        var users = await _userService.GetAllUsersAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(Guid id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return Ok(user);
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        Guid userId = _currentUserService.UserId ?? Guid.Empty;
        UserDto user = await _userService.GetUserByIdAsync(userId);
        return Ok(user);
    }

    [HttpGet("email/{email}")]
    public async Task<ActionResult<UserDto>> GetUserByEmail(string email)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(Guid id)
    {
        try
        {
            var result = await _userService.DeleteUserAsync(id);
            return Ok(result);
        }
        catch (InvalidOperationException)
        {
            return NotFound("User not found.");
        }
    }
}