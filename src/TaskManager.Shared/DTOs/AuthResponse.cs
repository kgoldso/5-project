namespace TaskManager.Shared.DTOs;

/// <summary>
/// Ответ сервера при успешной аутентификации.
/// </summary>
public record AuthResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    UserDto User);
