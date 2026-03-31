using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .HasIndex(t => t.ReferenceId)
            .IsUnique();

        modelBuilder.Entity<Wallet>()
            .HasIndex(t => t.UserId)
            .IsUnique();

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.FromWallet)
            .WithMany(w => w.SentTransactions)
            .HasForeignKey(t => t.FromWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.ToWallet)
            .WithMany(w => w.ReceivedTransactions)
            .HasForeignKey(t => t.ToWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Wallet>()
            .HasOne(w => w.User)
            .WithMany()
            .HasForeignKey(w => w.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LedgerEntry>()
            .HasOne(le => le.Transaction)
            .WithMany()
            .HasForeignKey(le => le.TransactionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LedgerEntry>()
            .HasOne(le => le.Wallet)
            .WithMany()
            .HasForeignKey(le => le.WalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany()
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RefreshToken>()
        .HasIndex(rt => rt.Token)
        .IsUnique();
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Wallet> Wallets => Set<Wallet>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
}