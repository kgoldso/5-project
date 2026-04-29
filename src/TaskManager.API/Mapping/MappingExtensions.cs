using TaskManager.Domain.Entities;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Mapping;

/// <summary>
/// Методы-расширения для маппинга между сущностями и DTO.
/// </summary>
public static class MappingExtensions
{
    /// <summary>Конвертирует сущность Process в DTO.</summary>
    public static ProcessDto ToDto(this Process process) => new(
        process.Id,
        process.Title,
        process.Description,
        process.CreatedAt,
        process.Tasks.Count);

    /// <summary>Конвертирует сущность TaskItem в DTO.</summary>
    public static TaskItemDto ToDto(this TaskItem task) => new(
        task.Id,
        task.Title,
        task.Description,
        task.Status,
        task.Priority,
        task.DueDate,
        task.ProcessId,
        task.CreatedAt);

    /// <summary>Конвертирует сущность User в DTO.</summary>
    public static UserDto ToDto(this User user) => new(
        user.Id,
        user.Username,
        user.Email,
        user.Role.ToString());
}
