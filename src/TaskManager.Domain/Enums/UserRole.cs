namespace TaskManager.Domain.Enums;

/// <summary>
/// Роль пользователя в системе.
/// </summary>
public enum UserRole
{
    /// <summary>Обычный пользователь.</summary>
    User = 0,

    /// <summary>Администратор с расширенными правами.</summary>
    Admin = 1
}
