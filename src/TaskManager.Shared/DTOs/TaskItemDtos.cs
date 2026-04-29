using TaskManager.Domain.Enums;

namespace TaskManager.Shared.DTOs;

/// <summary>
/// Информация о задаче (ответ API).
/// </summary>
public record TaskItemDto(
    Guid Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid ProcessId,
    DateTime CreatedAt);

/// <summary>
/// Данные для создания или обновления задачи.
/// </summary>
public record TaskItemCreateDto(
    string Title,
    string? Description,
    TaskItemStatus Status,
    TaskPriority Priority,
    DateTime? DueDate,
    Guid ProcessId);
