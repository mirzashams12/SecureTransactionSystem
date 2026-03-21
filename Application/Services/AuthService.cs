using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly ITokenRepository _tokenRepo;
    private readonly IDataService _dataService;
    private readonly IUserRepository _userRepo;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    public AuthService(ITokenService tokenService, ITokenRepository tokenRepository,
                        IDataService dataService, IUserRepository userRepository, IPasswordHasher passwordHasher,
                        IMapper mapper, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
    {
        _tokenService = tokenService;
        _tokenRepo = tokenRepository;
        _dataService = dataService;
        _userRepo = userRepository;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<AuthResponseDto> AuthenticateUserAsync(string email, string password)
    {
        try
        {
            User user = _userRepo.GetByEmailAsync(email).Result;

            if (_passwordHasher.VerifyPassword(user.PasswordHash, password))
            {
                TokenDto accessToken = _tokenService.GenerateToken(user.Id, user.Email);
                RefreshToken refreshToken = await AddRefreshTokenAsync(user.Id);

                await _unitOfWork.SaveChangesAsync();

                return new AuthResponseDto
                {
                    AccessToken = accessToken,
                    RefreshToken = _mapper.Map<RefreshTokenDto>(refreshToken),

                };
            }
            else
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }
        }
        catch (AggregateException ex) when (ex.InnerException is InvalidOperationException)
        {
            throw new UnauthorizedAccessException("Invalid user.");
        }
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(string token)
    {
        var now = _dataService.GetCurrentDate();

        // Start transaction (prevents race conditions)
        await _unitOfWork.BeginTransactionAsync();

        var existingToken = await _tokenRepo.GetByTokenAsync(token);

        // 1. Validate token existence
        if (existingToken == null)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        // 2. Detect reuse (CRITICAL SECURITY CHECK)
        if (existingToken.IsRevoked)
        {
            // 🚨 Token reuse detected → possible attack
            await _tokenRepo.RemoveAllForUserAsync(existingToken.UserId);

            await _unitOfWork.SaveChangesAsync();
            await _unitOfWork.CommitAsync();

            throw new UnauthorizedAccessException("Token reuse detected. All sessions revoked.");
        }

        // 3. Check expiry
        if (existingToken.Expiration < now)
            throw new UnauthorizedAccessException("Refresh token expired.");

        // 4. Revoke current token (rotation)
        existingToken.IsRevoked = true;
        existingToken.RevokedAt = now;

        var user = existingToken.User;

        // 5. Generate new access token
        var accessToken = _tokenService.GenerateToken(user.Id, user.Email);

        // 6. Generate new refresh token (NEW ROW)
        RefreshToken newRefreshToken = await AddRefreshTokenAsync(user.Id);

        // 7. Link tokens (token chaining)
        existingToken.ReplacedByToken = newRefreshToken.Token;

        await _tokenRepo.AddAsync(newRefreshToken);

        // 8. Save changes
        await _unitOfWork.SaveChangesAsync();

        // 9. Commit transaction
        await _unitOfWork.CommitAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = _mapper.Map<RefreshTokenDto>(newRefreshToken)
        };
    }

    public Task RevokeRefreshTokenAsync(string refreshToken)
    {
        throw new NotImplementedException();
    }

    public Task RevokeUserTokensAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    private async Task<RefreshToken> AddRefreshTokenAsync(Guid userId)
    {
        var now = _dataService.GetCurrentDate();
        string refreshTokenValue = _tokenService.GenerateRefreshToken();

        RefreshToken refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Token = refreshTokenValue,
            Expiration = now.AddDays(7),
            CreatedAt = now,
            CreatedByIp = _currentUserService.IpAddress,
            IsRevoked = false
        };
        await _tokenRepo.AddAsync(refreshToken);
        return refreshToken;
    }
}