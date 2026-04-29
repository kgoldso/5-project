using TaskManager.Client.Services;
using Xunit;

namespace TaskManager.Client.Tests.Services;

/// <summary>
/// Тесты хранилища токенов.
/// </summary>
public class TokenStorageServiceTests
{
    [Fact]
    public void SaveTokens_СохраняетТокены()
    {
        // Arrange
        var service = new TokenStorageService();

        // Act
        service.SaveTokens("access_token", "refresh_token");

        // Assert
        Assert.Equal("access_token", service.GetAccessToken());
        Assert.Equal("refresh_token", service.GetRefreshToken());
        Assert.True(service.HasTokens());
    }

    [Fact]
    public void ClearTokens_ОчищаетТокены()
    {
        // Arrange
        var service = new TokenStorageService();
        service.SaveTokens("access", "refresh");

        // Act
        service.ClearTokens();

        // Assert
        Assert.Null(service.GetAccessToken());
        Assert.Null(service.GetRefreshToken());
        Assert.False(service.HasTokens());
    }

    [Fact]
    public void HasTokens_ВозвращаетFalse_ЕслиТокеныНеСохранены()
    {
        // Arrange
        var service = new TokenStorageService();

        // Assert
        Assert.False(service.HasTokens());
    }
}
