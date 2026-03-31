using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly AppDbContext _context;

    public WalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet> GetByIdAsync(Guid walletId)
    {
        return await _context.Wallets.FindAsync(walletId) ?? throw new KeyNotFoundException("Wallet not found");
    }

    public async Task<IEnumerable<Wallet>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Wallets.AsNoTracking().Where(w => w.UserId == userId).ToListAsync();
    }

    public async Task AddAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
    }
}