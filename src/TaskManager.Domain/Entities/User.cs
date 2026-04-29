using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

/// <summary>
/// Пользователь системы с учётными данными для аутентификации.
/// </summary>
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Уникальное имя пользователя.</summary>
    public required string Username { get; set; }

    /// <summary>Электронная почта пользователя.</summary>
    public required string Email { get; set; }

    /// <summary>Хеш пароля (PBKDF2, Base64).</summary>
    public required string PasswordHash { get; set; }

    /// <summary>Роль пользователя в системе.</summary>
    public UserRole Role { get; set; } = UserRole.User;

    /// <summary>Дата регистрации.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Навигационное свойство — процессы пользователя.</summary>
    public ICollection<Process> Processes { get; set; } = [];

    /// <summary>Навигационное свойство — refresh-токены.</summary>
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
}
