using TaskManager.Client.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services;

/// <summary>
/// Клиентский сервис для работы с процессами через API.
/// </summary>
public class ProcessApiService(IApiClient apiClient) : IProcessApiService
{
    public async Task<IEnumerable<ProcessDto>> GetAllAsync()
        => await apiClient.GetAsync<IEnumerable<ProcessDto>>("api/processes") ?? [];

    public async Task<ProcessDto?> GetByIdAsync(Guid id)
        => await apiClient.GetAsync<ProcessDto>($"api/processes/{id}");

    public async Task<ProcessDto> CreateAsync(ProcessCreateDto dto)
        => await apiClient.PostAsync<ProcessDto>("api/processes", dto)
           ?? throw new InvalidOperationException("Не удалось создать процесс.");

    public async Task<ProcessDto> UpdateAsync(Guid id, ProcessCreateDto dto)
        => await apiClient.PutAsync<ProcessDto>($"api/processes/{id}", dto)
           ?? throw new InvalidOperationException("Не удалось обновить процесс.");

    public async Task DeleteAsync(Guid id)
        => await apiClient.DeleteAsync($"api/processes/{id}");
}
