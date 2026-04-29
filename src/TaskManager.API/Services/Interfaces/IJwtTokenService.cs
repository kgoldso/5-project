using System.Security.Claims;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Services.Interfaces;

/// <summary>
/// Сервис генерации и валидации JWT-токенов.
/// </summary>
public interface IJwtTokenService
{
    /// <summary>Генерирует access-токен для пользователя.</summary>
    string GenerateAccessToken(User user);

    /// <summary>Генерирует refresh-токен.</summary>
    string GenerateRefreshToken();

    /// <summary>Извлекает claims из просроченного access-токена.</summary>
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}
