using TaskManager.Shared.DTOs;

namespace TaskManager.API.Services.Interfaces;

/// <summary>
/// Сервис бизнес-логики процессов.
/// </summary>
public interface IProcessService
{
    Task<IEnumerable<ProcessDto>> GetAllByOwnerAsync(Guid ownerId);
    Task<ProcessDto?> GetByIdAsync(Guid processId);
    Task<ProcessDto> CreateAsync(Guid ownerId, ProcessCreateDto dto);
    Task<ProcessDto> UpdateAsync(Guid processId, Guid ownerId, ProcessCreateDto dto);
    Task DeleteAsync(Guid processId, Guid ownerId);
}
