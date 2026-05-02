using TaskManager.API.Mapping;
using TaskManager.API.Services.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Services;

public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    public async Task<IEnumerable<TaskItemDto>> GetByProcessIdAsync(Guid processId)
    {
        var tasks = await taskRepository.GetByProcessIdAsync(processId);
        return tasks.Select(t => t.ToDto());
    }

    public async Task<TaskItemDto?> GetByIdAsync(Guid taskId)
    {
        var task = await taskRepository.GetByIdAsync(taskId);
        return task?.ToDto();
    }

    public async Task<TaskItemDto> CreateAsync(TaskItemCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            Priority = dto.Priority,
            DueDate = dto.DueDate,
            ProcessId = dto.ProcessId
        };

        await taskRepository.AddAsync(task);
        await taskRepository.SaveChangesAsync();

        return task.ToDto();
    }

    public async Task<TaskItemDto> UpdateAsync(Guid taskId, TaskItemCreateDto dto)
    {
        var task = await taskRepository.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException("Задача не найдена.");

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.Status = dto.Status;
        task.Priority = dto.Priority;
        task.DueDate = dto.DueDate;

        taskRepository.Update(task);
        await taskRepository.SaveChangesAsync();

        return task.ToDto();
    }

    public async Task DeleteAsync(Guid taskId)
    {
        var task = await taskRepository.GetByIdAsync(taskId)
            ?? throw new KeyNotFoundException("Задача не найдена.");

        taskRepository.Remove(task);
        await taskRepository.SaveChangesAsync();
    }
}
