using Microsoft.Extensions.Options;
using TaskManager.API.Configuration;
using TaskManager.API.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.API.Tests.Services;

/// <summary>
/// Тесты сервиса JWT-токенов.
/// </summary>
public class JwtTokenServiceTests
{
    private readonly JwtTokenService _service;

    public JwtTokenServiceTests()
    {
        var settings = new JwtSettings
        {
            Secret = "TestSecretKeyForJwtTokenMinimum32Characters!!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 30,
            RefreshTokenExpirationDays = 7
        };

        _service = new JwtTokenService(Options.Create(settings));
    }

    [Fact]
    public void GenerateAccessToken_ВозвращаетНепустойТокен()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@test.com",
            PasswordHash = "hash",
            Role = UserRole.User
        };

        // Act
        var token = _service.GenerateAccessToken(user);

        // Assert
        Assert.NotNull(token);
        Assert.NotEmpty(token);
        Assert.Contains('.', token); // JWT содержит точки (header.payload.signature)
    }

    [Fact]
    public void GenerateRefreshToken_ВозвращаетУникальныеТокены()
    {
        // Act
        var token1 = _service.GenerateRefreshToken();
        var token2 = _service.GenerateRefreshToken();

        // Assert
        Assert.NotEmpty(token1);
        Assert.NotEmpty(token2);
        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ИзвлекаетClaimsИзВалидногоТокена()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = "testuser",
            Email = "test@test.com",
            PasswordHash = "hash",
            Role = UserRole.Admin
        };

        var token = _service.GenerateAccessToken(user);

        // Act
        var principal = _service.GetPrincipalFromExpiredToken(token);

        // Assert
        Assert.NotNull(principal);
        Assert.Contains(principal.Claims, c => c.Value == user.Username);
    }

    [Fact]
    public void GetPrincipalFromExpiredToken_ВозвращаетNull_ДляНевалидногоТокена()
    {
        // Act
        var principal = _service.GetPrincipalFromExpiredToken("invalid.token.value");

        // Assert
        Assert.Null(principal);
    }
}
