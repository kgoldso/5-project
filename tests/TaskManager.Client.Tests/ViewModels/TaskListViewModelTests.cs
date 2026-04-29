using Moq;
using TaskManager.Client.Services.Interfaces;
using TaskManager.Client.ViewModels;
using TaskManager.Domain.Enums;
using Xunit;

namespace TaskManager.Client.Tests.ViewModels;

/// <summary>
/// Тесты ViewModel списка задач.
/// </summary>
public class TaskListViewModelTests
{
    [Fact]
    public void NewTaskCommand_ОткрываетФормуСоЗначениямиПоУмолчанию()
    {
        // Arrange
        var taskMock = new Mock<ITaskApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new TaskListViewModel(taskMock.Object, regionMock.Object);

        // Act
        vm.NewTaskCommand.Execute();

        // Assert
        Assert.True(vm.IsEditing);
        Assert.Equal(string.Empty, vm.EditTitle);
        Assert.Equal(TaskItemStatus.NotStarted, vm.EditStatus);
        Assert.Equal(TaskPriority.Medium, vm.EditPriority);
        Assert.Null(vm.EditDueDate);
    }

    [Fact]
    public void Statuses_СодержитВсеЗначенияПеречисления()
    {
        // Arrange
        var taskMock = new Mock<ITaskApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new TaskListViewModel(taskMock.Object, regionMock.Object);

        // Assert
        Assert.Equal(4, vm.Statuses.Length);
        Assert.Contains(TaskItemStatus.InProgress, vm.Statuses);
    }

    [Fact]
    public void Priorities_СодержитВсеЗначенияПеречисления()
    {
        // Arrange
        var taskMock = new Mock<ITaskApiService>();
        var regionMock = new Mock<Prism.Regions.IRegionManager>();
        var vm = new TaskListViewModel(taskMock.Object, regionMock.Object);

        // Assert
        Assert.Equal(4, vm.Priorities.Length);
        Assert.Contains(TaskPriority.Critical, vm.Priorities);
    }
}
