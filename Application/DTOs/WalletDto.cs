namespace Application.DTOs;

public class WalletDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}