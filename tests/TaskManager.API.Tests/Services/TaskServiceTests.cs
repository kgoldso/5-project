using Moq;
using TaskManager.API.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.API.Tests.Services;

/// <summary>
/// Тесты сервиса задач.
/// </summary>
public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _repoMock = new();
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _service = new TaskService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_СоздаётЗадачу()
    {
        // Arrange
        var processId = Guid.NewGuid();
        var dto = new TaskItemCreateDto("Новая задача", "Описание",
            TaskItemStatus.NotStarted, TaskPriority.High, null, processId);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.Equal("Новая задача", result.Title);
        Assert.Equal(TaskPriority.High, result.Priority);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<TaskItem>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ОбновляетЗадачу()
    {
        // Arrange
        var taskId = Guid.NewGuid();
        var processId = Guid.NewGuid();
        var task = new TaskItem { Id = taskId, Title = "Старая", ProcessId = processId };
        _repoMock.Setup(r => r.GetByIdAsync(taskId)).ReturnsAsync(task);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        var dto = new TaskItemCreateDto("Обновлённая", "Новое описание",
            TaskItemStatus.InProgress, TaskPriority.Critical, DateTime.UtcNow.AddDays(7), processId);

        // Act
        var result = await _service.UpdateAsync(taskId, dto);

        // Assert
        Assert.Equal("Обновлённая", result.Title);
        Assert.Equal(TaskItemStatus.InProgress, result.Status);
    }

    [Fact]
    public async Task DeleteAsync_ВыбрасываетОшибку_ЕслиЗадачаНеНайдена()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((TaskItem?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetByProcessIdAsync_ВозвращаетЗадачиПроцесса()
    {
        // Arrange
        var processId = Guid.NewGuid();
        var tasks = new List<TaskItem>
        {
            new() { Title = "Задача 1", ProcessId = processId },
            new() { Title = "Задача 2", ProcessId = processId }
        };
        _repoMock.Setup(r => r.GetByProcessIdAsync(processId)).ReturnsAsync(tasks);

        // Act
        var result = (await _service.GetByProcessIdAsync(processId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }
}
