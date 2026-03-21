using Application.DTOs;

namespace Application.Interfaces;

public interface ITokenService
{
    TokenDto GenerateToken(Guid userId, string email);
    string GenerateRefreshToken();
}