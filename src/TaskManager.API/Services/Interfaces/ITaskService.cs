using TaskManager.Shared.DTOs;

namespace TaskManager.API.Services.Interfaces;

/// <summary>
/// Сервис бизнес-логики задач.
/// </summary>
public interface ITaskService
{
    Task<IEnumerable<TaskItemDto>> GetByProcessIdAsync(Guid processId);
    Task<TaskItemDto?> GetByIdAsync(Guid taskId);
    Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto);
    Task<TaskItemDto> UpdateAsync(Guid taskId, TaskItemCreateDto dto);
    Task DeleteAsync(Guid taskId);
}
