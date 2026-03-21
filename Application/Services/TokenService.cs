using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly string _secretKey;

    public TokenService(IOptions<TokenSettings> tokenSettings)
    {
        _secretKey = tokenSettings.Value.Key ?? throw new ArgumentNullException("Jwt:Key configuration is missing.");
    }

    public TokenDto GenerateToken(Guid userId, string email)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_secretKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(
            [
                new System.Security.Claims.Claim("id", userId.ToString()),
                new System.Security.Claims.Claim("email", email)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new TokenDto
        {
            Token = tokenHandler.WriteToken(token),
            Expiration = tokenDescriptor.Expires ?? DateTime.UtcNow.AddMinutes(10)
        };
    }

    public string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }
}
