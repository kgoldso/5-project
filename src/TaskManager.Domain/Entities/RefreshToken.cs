namespace TaskManager.Domain.Entities;

/// <summary>
/// Refresh-токен для обновления JWT access-токена.
/// </summary>
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Значение токена.</summary>
    public required string Token { get; set; }

    /// <summary>Дата истечения срока действия.</summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>Дата создания.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Признак отзыва (revoke) токена.</summary>
    public bool IsRevoked { get; set; }

    /// <summary>Внешний ключ пользователя.</summary>
    public Guid UserId { get; set; }

    /// <summary>Навигационное свойство — владелец токена.</summary>
    public User? User { get; set; }
}
