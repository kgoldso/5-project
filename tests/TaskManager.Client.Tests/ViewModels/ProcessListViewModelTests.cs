using Moq;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Client.ViewModels;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.Client.Tests.ViewModels;

/// <summary>
/// Тесты ViewModel списка процессов.
/// </summary>
public class ProcessListViewModelTests
{
    [Fact]
    public void NewProcessCommand_ОткрываетФормуРедактирования()
    {
        // Arrange
        var processMock = new Mock<IProcessApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new ProcessListViewModel(processMock.Object, regionMock.Object);

        // Act
        vm.NewProcessCommand.Execute();

        // Assert
        Assert.True(vm.IsEditing);
        Assert.Equal(string.Empty, vm.EditTitle);
    }

    [Fact]
    public void EditProcessCommand_ЗаполняетФормуДанными()
    {
        // Arrange
        var processMock = new Mock<IProcessApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new ProcessListViewModel(processMock.Object, regionMock.Object);

        var process = new ProcessDto(Guid.NewGuid(), "Проект", "Описание", DateTime.UtcNow, 5);

        // Act
        vm.EditProcessCommand.Execute(process);

        // Assert
        Assert.True(vm.IsEditing);
        Assert.Equal("Проект", vm.EditTitle);
        Assert.Equal("Описание", vm.EditDescription);
    }

    [Fact]
    public void CancelEditCommand_ЗакрываетФормуРедактирования()
    {
        // Arrange
        var processMock = new Mock<IProcessApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new ProcessListViewModel(processMock.Object, regionMock.Object)
        {
            IsEditing = true
        };

        // Act
        vm.CancelEditCommand.Execute();

        // Assert
        Assert.False(vm.IsEditing);
    }
}
