namespace Application.DTOs;

public class AuthResponseDto
{
    public TokenDto AccessToken { get; set; } = new TokenDto();
    public RefreshTokenDto RefreshToken { get; set; } = new RefreshTokenDto();
}