using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Data;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Реализация репозитория refresh-токенов.
/// </summary>
public class RefreshTokenRepository(AppDbContext context) : Repository<RefreshToken>(context), IRefreshTokenRepository
{
    public async Task<RefreshToken?> GetByTokenAsync(string token)
        => await DbSet.FirstOrDefaultAsync(rt => rt.Token == token && !rt.IsRevoked);

    public async Task RevokeAllForUserAsync(Guid userId)
    {
        var tokens = await DbSet
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ToListAsync();

        foreach (var token in tokens)
            token.IsRevoked = true;
    }
}
