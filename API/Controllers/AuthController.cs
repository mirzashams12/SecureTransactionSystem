using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;

    public AuthController(IAuthService authService, IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login(UserAuthenticateDto userDto)
    {
        try
        {
            AuthResponseDto authResponse = await _authService.AuthenticateUserAsync(userDto.Email, userDto.Password);

            await StoreRefreshTokenInCookie(authResponse.RefreshToken);

            return Ok(authResponse.AccessToken);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid credentials.");
        }
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenDto>> RefreshToken()
    {
        try
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            AuthResponseDto authResponse = await _authService.RefreshTokenAsync(refreshToken);

            await StoreRefreshTokenInCookie(authResponse.RefreshToken);

            return Ok(authResponse.AccessToken);
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid refresh token.");
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser(UserCreateDto userDto)
    {
        try
        {
            var user = await _userService.CreateUserAsync(userDto);
            return Ok(user);
        }
        catch (DbUpdateException ex)
        when (ex.InnerException is SqlException sqlEx &&
              (sqlEx.Number == 2627 || sqlEx.Number == 2601))
        {
            return BadRequest("User already exists");
        }
        catch (Exception)
        {
            return StatusCode(500, "Something went wrong");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        string? refreshToken = Request.Cookies["refreshToken"];

        await _authService.RevokeRefreshTokenAsync(refreshToken);

        Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    [HttpPost("logout/all")]
    public async Task<IActionResult> LogoutForEveryone()
    {
        await _authService.RevokeUserTokensAsync();

        Response.Cookies.Delete("refreshToken");

        return Ok();
    }

    private async Task StoreRefreshTokenInCookie(RefreshTokenDto refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = false,
            SameSite = SameSiteMode.Lax,
            Expires = refreshToken.Expiration,
            Path = "/"
        };

        Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
    }
}