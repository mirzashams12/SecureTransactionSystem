using Domain.Entities;

namespace Application.Interfaces;

public interface IWalletRepository
{
    Task<Wallet> GetByIdAsync(Guid walletId);
    Task<IEnumerable<Wallet>> GetByUserIdAsync(Guid userId);
    Task AddAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
}