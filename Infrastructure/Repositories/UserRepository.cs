using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.AsNoTracking().Select(s => new User { Id = s.Id, Name = s.Name, Email = s.Email, CreatedAt = s.CreatedAt, UpdatedAt = s.UpdatedAt }).ToListAsync();
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new User { Id = user.Id, Name = user.Name, Email = user.Email, CreatedAt = user.CreatedAt, UpdatedAt = user.UpdatedAt };
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        User? user = await _context.Users.AsNoTracking().Select(u => new User { Id = u.Id, Name = u.Name, Email = u.Email, PasswordHash = u.PasswordHash }).FirstOrDefaultAsync(u => u.Email == email);

        return user ?? throw new InvalidOperationException("User not found.");
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User> GetByIdAsync(Guid id)
    {
        User? user = await _context.Users.AsNoTracking().Select(u => new User { Id = u.Id, Name = u.Name, Email = u.Email, CreatedAt = u.CreatedAt, UpdatedAt = u.UpdatedAt }).FirstOrDefaultAsync(u => u.Id == id);

        return user ?? throw new InvalidOperationException("User not found.");
    }

    public async Task<User> UpdateAsync(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException("User not found.");
        }

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = user.UpdatedAt;

        _context.Users.Update(existingUser);
        await _context.SaveChangesAsync();

        return new User { Id = existingUser.Id, Name = existingUser.Name, Email = existingUser.Email, CreatedAt = existingUser.CreatedAt, UpdatedAt = existingUser.UpdatedAt };
    }
}