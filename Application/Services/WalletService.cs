using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class WalletService : IWalletService
{
    private readonly IGuidService _guidService;
    private readonly IWalletRepository _repo;
    private readonly IDataService _dateService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public WalletService(IGuidService guidService,
                        IWalletRepository repo,
                        IDataService dateService,
                        IUnitOfWork unitOfWork,
                        IMapper mapper)
    {
        _guidService = guidService;
        _repo = repo;
        _dateService = dateService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<WalletDto> CreateWalletAsync(Guid userId)
    {

        Wallet wallet = new Wallet
        {
            Id = _guidService.GetGuid(),
            UserId = userId,
            CreatedAt = _dateService.GetCurrentDate(),
            UpdatedAt = _dateService.GetCurrentDate()
        };

        await _repo.AddAsync(wallet);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task<IEnumerable<WalletDto>> GetUserWalletsAsync(Guid userId)
    {
        IEnumerable<Wallet> wallets = await _repo.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<WalletDto>>(wallets);
    }

    public async Task<WalletDto> GetWalletByIdAsync(Guid walletId)
    {
        Wallet wallet = await _repo.GetByIdAsync(walletId);
        return _mapper.Map<WalletDto>(wallet);
    }

    public async Task UpdateWalletAsync(Guid walletId)
    {
        Wallet wallet = await _repo.GetByIdAsync(walletId);
        wallet.UpdatedAt = _dateService.GetCurrentDate();
        await _repo.UpdateAsync(wallet);
        await _unitOfWork.SaveChangesAsync();
    }
}