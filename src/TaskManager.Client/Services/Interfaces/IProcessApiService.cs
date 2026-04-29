using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services.Interfaces;

/// <summary>
/// Клиентский сервис для работы с процессами через API.
/// </summary>
public interface IProcessApiService
{
    Task<IEnumerable<ProcessDto>> GetAllAsync();
    Task<ProcessDto?> GetByIdAsync(Guid id);
    Task<ProcessDto> CreateAsync(ProcessCreateDto dto);
    Task<ProcessDto> UpdateAsync(Guid id, ProcessCreateDto dto);
    Task DeleteAsync(Guid id);
}
