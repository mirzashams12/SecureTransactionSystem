namespace Domain.Entities;

public class LedgerEntry
{
    public Guid Id { get; set; }
    public Guid TransactionId { get; set; }
    public Transaction Transaction { get; set; } = default!;
    public Guid WalletId { get; set; }
    public Wallet Wallet { get; set; } = default!;
    public decimal Amount { get; set; }
    public LedgerEntryType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}

public enum LedgerEntryType
{
    Debit,
    Credit
}