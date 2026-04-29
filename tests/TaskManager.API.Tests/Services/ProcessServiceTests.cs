using Moq;
using TaskManager.API.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.API.Tests.Services;

/// <summary>
/// Тесты сервиса процессов.
/// </summary>
public class ProcessServiceTests
{
    private readonly Mock<IProcessRepository> _repoMock = new();
    private readonly ProcessService _service;

    public ProcessServiceTests()
    {
        _service = new ProcessService(_repoMock.Object);
    }

    [Fact]
    public async Task CreateAsync_СоздаётПроцесс()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var dto = new ProcessCreateDto("Тестовый проект", "Описание");
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        var result = await _service.CreateAsync(ownerId, dto);

        // Assert
        Assert.Equal("Тестовый проект", result.Title);
        Assert.Equal("Описание", result.Description);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Process>()), Times.Once);
    }

    [Fact]
    public async Task GetAllByOwnerAsync_ВозвращаетСписокПроцессов()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var processes = new List<Process>
        {
            new() { Title = "Проект 1", OwnerId = ownerId, Tasks = [] },
            new() { Title = "Проект 2", OwnerId = ownerId, Tasks = [] }
        };
        _repoMock.Setup(r => r.GetAllByOwnerAsync(ownerId)).ReturnsAsync(processes);

        // Act
        var result = (await _service.GetAllByOwnerAsync(ownerId)).ToList();

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateAsync_ВыбрасываетОшибку_ЕслиПроцессНеНайден()
    {
        // Arrange
        _repoMock.Setup(r => r.GetWithTasksAsync(It.IsAny<Guid>())).ReturnsAsync((Process?)null);

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(Guid.NewGuid(), Guid.NewGuid(), new ProcessCreateDto("Test", null)));
    }

    [Fact]
    public async Task UpdateAsync_ВыбрасываетОшибку_ЕслиНетДоступа()
    {
        // Arrange
        var processId = Guid.NewGuid();
        var process = new Process { Id = processId, Title = "Test", OwnerId = Guid.NewGuid(), Tasks = [] };
        _repoMock.Setup(r => r.GetWithTasksAsync(processId)).ReturnsAsync(process);

        // Act & Assert
        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            _service.UpdateAsync(processId, Guid.NewGuid(), new ProcessCreateDto("New", null)));
    }

    [Fact]
    public async Task DeleteAsync_УдаляетПроцесс()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var processId = Guid.NewGuid();
        var process = new Process { Id = processId, Title = "Test", OwnerId = ownerId, Tasks = [] };
        _repoMock.Setup(r => r.GetByIdAsync(processId)).ReturnsAsync(process);
        _repoMock.Setup(r => r.SaveChangesAsync()).ReturnsAsync(1);

        // Act
        await _service.DeleteAsync(processId, ownerId);

        // Assert
        _repoMock.Verify(r => r.Remove(process), Times.Once);
    }
}
