using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(UserAuthenticateDto userDto)
    {
        try
        {
            var authResponse = await _authService.AuthenticateUserAsync(userDto.Email, userDto.Password);
            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken(TokenRequestDto request)
    {
        try
        {
            var authResponse = await _authService.RefreshTokenAsync(request.RefreshToken);
            return Ok(authResponse);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid refresh token.");
        }
    }
}