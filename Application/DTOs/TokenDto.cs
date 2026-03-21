namespace Application.DTOs;

public class TokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}

public class TokenSettings
{
    public string Key { get; set; } = string.Empty;
}

public class RefreshTokenDto
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
    public Guid UserId { get; set; }
}

public class TokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}