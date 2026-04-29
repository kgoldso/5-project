using System.Security.Claims;
using System.Security.Cryptography;
using TaskManager.API.Configuration;
using TaskManager.API.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;
using Microsoft.Extensions.Options;

namespace TaskManager.API.Services;

/// <summary>
/// Сервис аутентификации: регистрация, вход и обновление токенов.
/// Использует PBKDF2 для хеширования паролей.
/// </summary>
public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsAsync(request.Username))
            throw new InvalidOperationException("Пользователь с таким именем уже существует.");

        if (await userRepository.GetByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Пользователь с таким email уже существует.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = HashPassword(request.Password)
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return await GenerateAuthResponseAsync(user);
    }

    /// <summary>
    /// Аутентификация пользователя по имени и паролю.
    /// </summary>
    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Неверное имя пользователя или пароль.");

        if (!VerifyPassword(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверное имя пользователя или пароль.");

        return await GenerateAuthResponseAsync(user);
    }

    /// <summary>
    /// Обновление пары access + refresh токенов.
    /// </summary>
    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await refreshTokenRepository.GetByTokenAsync(refreshToken)
            ?? throw new UnauthorizedAccessException("Недействительный refresh-токен.");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            storedToken.IsRevoked = true;
            await refreshTokenRepository.SaveChangesAsync();
            throw new UnauthorizedAccessException("Refresh-токен истёк.");
        }

        storedToken.IsRevoked = true;

        var user = await userRepository.GetByIdAsync(storedToken.UserId)
            ?? throw new UnauthorizedAccessException("Пользователь не найден.");

        var response = await GenerateAuthResponseAsync(user);
        await refreshTokenRepository.SaveChangesAsync();

        return response;
    }

    /// <summary>
    /// Формирует ответ аутентификации с парой токенов.
    /// </summary>
    private async Task<AuthResponse> GenerateAuthResponseAsync(User user)
    {
        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshTokenValue = jwtTokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };

        await refreshTokenRepository.AddAsync(refreshToken);
        await refreshTokenRepository.SaveChangesAsync();

        var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes);

        return new AuthResponse(
            accessToken,
            refreshTokenValue,
            expiresAt,
            new UserDto(user.Id, user.Username, user.Email, user.Role.ToString()));
    }

    /// <summary>
    /// Хеширует пароль с помощью PBKDF2 (SHA-256, 100 000 итераций).
    /// </summary>
    private static string HashPassword(string password)
    {
        var salt = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

        var combined = new byte[salt.Length + hash.Length];
        Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
        Buffer.BlockCopy(hash, 0, combined, salt.Length, hash.Length);

        return Convert.ToBase64String(combined);
    }

    /// <summary>
    /// Проверяет пароль против сохранённого хеша.
    /// </summary>
    private static bool VerifyPassword(string password, string storedHash)
    {
        var combined = Convert.FromBase64String(storedHash);
        var salt = combined[..16];
        var storedHashBytes = combined[16..];

        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 100_000, HashAlgorithmName.SHA256, 32);

        return CryptographicOperations.FixedTimeEquals(hash, storedHashBytes);
    }
}
