namespace TaskManager.Shared.DTOs;

/// <summary>
/// Запрос на вход в систему.
/// </summary>
public record LoginRequest(string Username, string Password);

/// <summary>
/// Запрос на регистрацию нового пользователя.
/// </summary>
public record RegisterRequest(string Username, string Email, string Password);

/// <summary>
/// Запрос на обновление пары токенов.
/// </summary>
public record RefreshTokenRequest(string RefreshToken);
