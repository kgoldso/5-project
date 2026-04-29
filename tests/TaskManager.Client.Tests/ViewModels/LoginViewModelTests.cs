using Moq;
using TaskManager.Client.Services;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.Client.Tests.ViewModels;

/// <summary>
/// Тесты ViewModel авторизации.
/// </summary>
public class LoginViewModelTests
{
    [Fact]
    public void LoginCommand_НедоступнаПриПустыхПолях()
    {
        // Arrange
        var authMock = new Mock<IAuthApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new Client.ViewModels.LoginViewModel(authMock.Object, regionMock.Object);

        // Act
        vm.Username = "";
        vm.Password = "";

        // Assert
        Assert.False(vm.LoginCommand.CanExecute());
    }

    [Fact]
    public void LoginCommand_ДоступнаПриЗаполненныхПолях()
    {
        // Arrange
        var authMock = new Mock<IAuthApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new Client.ViewModels.LoginViewModel(authMock.Object, regionMock.Object);

        // Act
        vm.Username = "testuser";
        vm.Password = "password";

        // Assert
        Assert.True(vm.LoginCommand.CanExecute());
    }
}
