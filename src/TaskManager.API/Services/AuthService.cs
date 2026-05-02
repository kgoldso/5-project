using System.Security.Claims;
using TaskManager.API.Configuration;
using TaskManager.API.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;
using Microsoft.Extensions.Options;

namespace TaskManager.API.Services;

public class AuthService(
    IUserRepository userRepository,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenService jwtTokenService,
    IOptions<JwtSettings> jwtOptions) : IAuthService
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
    {
        if (await userRepository.ExistsAsync(request.Username))
            throw new InvalidOperationException("Имя пользователя уже занято");

        if (await userRepository.GetByEmailAsync(request.Email) is not null)
            throw new InvalidOperationException("Email уже используется");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = PasswordHasher.Hash(request.Password)
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return await CreateAuthResponse(user);
    }

    public async Task<AuthResponse> LoginAsync(LoginRequest request)
    {
        var user = await userRepository.GetByUsernameAsync(request.Username)
            ?? throw new UnauthorizedAccessException("Неверный логин или пароль");

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Неверный логин или пароль");

        return await CreateAuthResponse(user);
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await refreshTokenRepository.GetByTokenAsync(refreshToken)
            ?? throw new UnauthorizedAccessException("Токен не найден");

        if (storedToken.ExpiresAt < DateTime.UtcNow)
        {
            storedToken.IsRevoked = true;
            await refreshTokenRepository.SaveChangesAsync();
            throw new UnauthorizedAccessException("Сессия истекла");
        }

        storedToken.IsRevoked = true;
        var user = await userRepository.GetByIdAsync(storedToken.UserId)
            ?? throw new UnauthorizedAccessException("Пользователь потерян");

        var response = await CreateAuthResponse(user);
        await refreshTokenRepository.SaveChangesAsync();

        return response;
    }

    private async Task<AuthResponse> CreateAuthResponse(User user)
    {
        var accessToken = jwtTokenService.GenerateAccessToken(user);
        var refreshTokenValue = jwtTokenService.GenerateRefreshToken();

        await refreshTokenRepository.AddAsync(new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        });
        await refreshTokenRepository.SaveChangesAsync();

        return new AuthResponse(
            accessToken,
            refreshTokenValue,
            DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpirationMinutes),
            new UserDto(user.Id, user.Username, user.Email, user.Role.ToString()));
    }
}
