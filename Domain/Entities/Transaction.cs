namespace Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
    public Guid FromWalletId { get; set; }
    public Wallet FromWallet { get; set; } = default!;
    public Guid ToWalletId { get; set; }
    public Wallet ToWallet { get; set; } = default!;
    public decimal Amount { get; set; }
    public TransactionStatus Status { get; set; } = TransactionStatus.Pending;
    public string ReferenceId { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public enum TransactionStatus
{
    Pending,
    Completed,
    Failed
}