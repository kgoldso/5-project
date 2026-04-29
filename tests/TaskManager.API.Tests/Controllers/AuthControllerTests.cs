using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskManager.API.Controllers;
using TaskManager.API.Services.Interfaces;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.API.Tests.Controllers;

/// <summary>
/// Тесты контроллера аутентификации.
/// </summary>
public class AuthControllerTests
{
    private readonly Mock<IAuthService> _authMock = new();
    private readonly AuthController _controller;

    public AuthControllerTests()
    {
        _controller = new AuthController(_authMock.Object);
    }

    [Fact]
    public async Task Register_Возвращает200_ПриУспешнойРегистрации()
    {
        // Arrange
        var request = new RegisterRequest("user", "user@test.com", "password123");
        var response = new AuthResponse("token", "refresh", DateTime.UtcNow.AddMinutes(30),
            new UserDto(Guid.NewGuid(), "user", "user@test.com", "User"));

        _authMock.Setup(s => s.RegisterAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Register_Возвращает400_ПриПустыхПолях()
    {
        // Arrange
        var request = new RegisterRequest("", "", "");

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_Возвращает400_ПриПустыхПолях()
    {
        // Arrange
        var request = new LoginRequest("", "");

        // Act
        var result = await _controller.Login(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_Возвращает200_ПриУспешномВходе()
    {
        // Arrange
        var request = new LoginRequest("user", "password");
        var response = new AuthResponse("token", "refresh", DateTime.UtcNow.AddMinutes(30),
            new UserDto(Guid.NewGuid(), "user", "user@test.com", "User"));

        _authMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(response);

        // Act
        var result = await _controller.Login(request);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}
