using TaskManager.API.Mapping;
using TaskManager.API.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Services;

/// <summary>
/// Сервис для управления бизнес-процессами.
/// </summary>
public class ProcessService(IProcessRepository processRepository) : IProcessService
{
    public async Task<IEnumerable<ProcessDto>> GetAllByOwnerAsync(Guid ownerId)
    {
        var processes = await processRepository.GetAllByOwnerAsync(ownerId);
        return processes.Select(p => p.ToDto());
    }

    public async Task<ProcessDto?> GetByIdAsync(Guid processId)
    {
        var process = await processRepository.GetWithTasksAsync(processId);
        return process?.ToDto();
    }

    public async Task<ProcessDto> CreateAsync(Guid ownerId, ProcessCreateDto dto)
    {
        var process = new Process
        {
            Title = dto.Title,
            Description = dto.Description,
            OwnerId = ownerId
        };

        await processRepository.AddAsync(process);
        await processRepository.SaveChangesAsync();

        return process.ToDto();
    }

    public async Task<ProcessDto> UpdateAsync(Guid processId, Guid ownerId, ProcessCreateDto dto)
    {
        var process = await processRepository.GetWithTasksAsync(processId)
            ?? throw new KeyNotFoundException("Процесс не найден.");

        if (process.OwnerId != ownerId)
            throw new UnauthorizedAccessException("У вас нет прав для изменения этого процесса.");

        process.Title = dto.Title;
        process.Description = dto.Description;

        processRepository.Update(process);
        await processRepository.SaveChangesAsync();

        return process.ToDto();
    }

    public async Task DeleteAsync(Guid processId, Guid ownerId)
    {
        var process = await processRepository.GetByIdAsync(processId)
            ?? throw new KeyNotFoundException("Процесс не найден.");

        if (process.OwnerId != ownerId)
            throw new UnauthorizedAccessException("У вас нет прав для удаления этого процесса.");

        processRepository.Remove(process);
        await processRepository.SaveChangesAsync();
    }
}
