using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с пользователями.
/// </summary>
public interface IUserRepository : IRepository<User>
{
    /// <summary>Найти пользователя по имени.</summary>
    Task<User?> GetByUsernameAsync(string username);

    /// <summary>Найти пользователя по email.</summary>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Проверить существование пользователя с указанным именем.</summary>
    Task<bool> ExistsAsync(string username);
}
