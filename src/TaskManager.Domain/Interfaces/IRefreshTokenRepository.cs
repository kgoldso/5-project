using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с refresh-токенами.
/// </summary>
public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    /// <summary>Найти активный токен по значению.</summary>
    Task<RefreshToken?> GetByTokenAsync(string token);

    /// <summary>Отозвать все токены пользователя.</summary>
    Task RevokeAllForUserAsync(Guid userId);
}
