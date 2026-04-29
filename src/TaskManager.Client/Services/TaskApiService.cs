using TaskManager.Client.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services;

/// <summary>
/// Клиентский сервис для работы с задачами через API.
/// </summary>
public class TaskApiService(IApiClient apiClient) : ITaskApiService
{
    public async Task<IEnumerable<TaskItemDto>> GetByProcessIdAsync(Guid processId)
        => await apiClient.GetAsync<IEnumerable<TaskItemDto>>($"api/tasks/by-process/{processId}") ?? [];

    public async Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto)
        => await apiClient.PostAsync<TaskItemDto>("api/tasks", dto)
           ?? throw new InvalidOperationException("Не удалось создать задачу.");

    public async Task<TaskItemDto> UpdateAsync(Guid id, TaskItemCreateDto dto)
        => await apiClient.PutAsync<TaskItemDto>($"api/tasks/{id}", dto)
           ?? throw new InvalidOperationException("Не удалось обновить задачу.");

    public async Task DeleteAsync(Guid id)
        => await apiClient.DeleteAsync($"api/tasks/{id}");
}
