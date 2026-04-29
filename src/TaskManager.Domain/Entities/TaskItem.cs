using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Entities;

/// <summary>
/// Задача (баг, таск) внутри процесса.
/// </summary>
public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Заголовок задачи.</summary>
    public required string Title { get; set; }

    /// <summary>Подробное описание.</summary>
    public string? Description { get; set; }

    /// <summary>Текущий статус выполнения.</summary>
    public TaskItemStatus Status { get; set; } = TaskItemStatus.NotStarted;

    /// <summary>Приоритет задачи.</summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;

    /// <summary>Срок выполнения.</summary>
    public DateTime? DueDate { get; set; }

    /// <summary>Дата создания.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Внешний ключ родительского процесса.</summary>
    public Guid ProcessId { get; set; }

    /// <summary>Навигационное свойство — родительский процесс.</summary>
    public Process? Process { get; set; }
}
