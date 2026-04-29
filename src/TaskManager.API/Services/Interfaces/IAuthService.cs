using TaskManager.Shared.DTOs;

namespace TaskManager.API.Services.Interfaces;

/// <summary>
/// Сервис аутентификации и регистрации пользователей.
/// </summary>
public interface IAuthService
{
    /// <summary>Регистрация нового пользователя.</summary>
    Task<AuthResponse> RegisterAsync(RegisterRequest request);

    /// <summary>Вход пользователя в систему.</summary>
    Task<AuthResponse> LoginAsync(LoginRequest request);

    /// <summary>Обновление пары access + refresh токенов.</summary>
    Task<AuthResponse> RefreshTokenAsync(string refreshToken);
}
