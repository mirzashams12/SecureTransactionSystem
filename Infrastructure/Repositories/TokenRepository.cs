using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class TokenRepository : ITokenRepository
{
    private readonly AppDbContext _context;

    public TokenRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        return await _context.RefreshTokens.Include(s => s.User).FirstOrDefaultAsync(rt => rt.Token == token) ?? throw new InvalidOperationException("Refresh token not found.");
    }

    public async Task RemoveAllForUserAsync(Guid userId)
    {
        List<RefreshToken> tokens = await _context.RefreshTokens.Where(rt => rt.UserId == userId && !rt.IsRevoked).ToListAsync();

        foreach (var token in tokens)
        {
            token.IsRevoked = true;
        }
    }

    public async Task AddAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
    }
}