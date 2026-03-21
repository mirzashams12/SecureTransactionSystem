using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> AuthenticateUserAsync(string email, string password);
    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
    Task RevokeUserTokensAsync(Guid userId);
}