using TaskManager.Domain.Entities;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Mapping;

public static class MappingExtensions
{
    public static ProcessDto ToDto(this Process process) => new(
        process.Id,
        process.Title,
        process.Description,
        process.CreatedAt,
        process.Tasks.Count);

    public static TaskItemDto ToDto(this TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.Priority,
        task.DueDate,
        task.ProcessId,
        task.CreatedAt);

    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Username,
        user.Email,
        user.Role.ToString());
}
