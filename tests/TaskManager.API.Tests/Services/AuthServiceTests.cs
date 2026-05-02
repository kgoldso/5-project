using Microsoft.Extensions.Options;
using Moq;
using TaskManager.API.Configuration;
using TaskManager.API.Services;
using TaskManager.API.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.API.Tests.Services;

/// <summary>
/// Тесты для сервиса аутентификации.
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IRefreshTokenRepository> _refreshRepoMock = new();
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IAuthService _authService;

    public AuthServiceTests()
    {
        var jwtSettings = new JwtSettings
        {
            Secret = "TestSecretKeyForJwtTokenMinimum32Characters!!",
            Issuer = "TestIssuer",
            Audience = "TestAudience",
            AccessTokenExpirationMinutes = 30,
            RefreshTokenExpirationDays = 7
        };

        var options = Options.Create(jwtSettings);
        _jwtTokenService = new JwtTokenService(options);

        _authService = new AuthService(
            _userRepoMock.Object,
            _refreshRepoMock.Object,
            _jwtTokenService,
            options);
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateNewUser_WhenDataIsValid()
    {
        // Arrange
        _userRepoMock.Setup(r => r.ExistsAsync("testuser")).ReturnsAsync(false);
        _userRepoMock.Setup(r => r.GetByEmailAsync("test@test.com")).ReturnsAsync((User?)null);
        _userRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);
        _refreshRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var request = new RegisterRequest("testuser", "test@test.com", "password123");

        // Act
        var result = await _authService.RegisterAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.NotEmpty(result.RefreshToken);
        Assert.Equal("testuser", result.User.Username);
        _userRepoMock.Verify(r => r.AddAsync(It.IsAny<User>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenUserAlreadyExists()
    {
        // Arrange
        _userRepoMock.Setup(r => r.ExistsAsync("existing")).ReturnsAsync(true);
        var request = new RegisterRequest("existing", "test@test.com", "password123");

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _authService.RegisterAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnTokens_WhenCredentialsAreCorrect()
    {
        // Arrange
        var user = CreateTestUser("testuser", "password123");
        _userRepoMock.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync(user);
        _refreshRepoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var request = new LoginRequest("testuser", "password123");

        // Act
        var result = await _authService.LoginAsync(request);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result.AccessToken);
        Assert.Equal("testuser", result.User.Username);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenPasswordIsIncorrect()
    {
        // Arrange
        var user = CreateTestUser("testuser", "correctpassword");
        _userRepoMock.Setup(r => r.GetByUsernameAsync("testuser")).ReturnsAsync(user);

        var request = new LoginRequest("testuser", "wrongpassword");

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserIsNotFound()
    {
        // Arrange
        _userRepoMock.Setup(r => r.GetByUsernameAsync("nonexistent")).ReturnsAsync((User?)null);
        var request = new LoginRequest("nonexistent", "password");

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _authService.LoginAsync(request));
    }

    // Вспомогательный метод для создания пользователя с хешем пароля
    private static User CreateTestUser(string username, string password)
    {
        // Используем рефлексию для доступа к приватному статическому методу HashPassword
        var method = typeof(AuthService).GetMethod("HashPassword",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!;

        var hash = (string)method.Invoke(null, [password])!;

        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = $"{username}@test.com",
            PasswordHash = hash,
            Role = UserRole.User
        };
    }
}
