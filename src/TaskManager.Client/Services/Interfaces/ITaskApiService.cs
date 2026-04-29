using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services.Interfaces;

/// <summary>
/// Клиентский сервис для работы с задачами через API.
/// </summary>
public interface ITaskApiService
{
    Task<IEnumerable<TaskItemDto>> GetByProcessIdAsync(Guid processId);
    Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto);
    Task<TaskItemDto> UpdateAsync(Guid id, TaskItemCreateDto dto);
    Task DeleteAsync(Guid id);
}
