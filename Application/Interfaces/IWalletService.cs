using Application.DTOs;

namespace Application.Interfaces;

public interface IWalletService
{
    Task<WalletDto> CreateWalletAsync(Guid userId);
    Task<IEnumerable<WalletDto>> GetUserWalletsAsync(Guid userId);
    Task<WalletDto> GetWalletByIdAsync(Guid walletId);
    Task UpdateWalletAsync(Guid walletId);
}