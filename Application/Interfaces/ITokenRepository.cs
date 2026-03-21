using Domain.Entities;

namespace Application.Interfaces;

public interface ITokenRepository
{
    Task AddAsync(RefreshToken refreshToken);
    Task<RefreshToken> GetByTokenAsync(string token);
    Task RemoveAllForUserAsync(Guid userId);
}