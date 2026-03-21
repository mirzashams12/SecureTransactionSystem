namespace Domain.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime RevokedAt { get; set; }
    public string? ReplacedByToken { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public string? CreatedByIp { get; set; }
    public DateTime CreatedAt { get; set; }
}