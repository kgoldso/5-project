namespace TaskManager.Domain.Entities;

/// <summary>
/// Процесс (проект), объединяющий связанные задачи.
/// </summary>
public class Process
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>Название процесса.</summary>
    public required string Title { get; set; }

    /// <summary>Описание процесса.</summary>
    public string? Description { get; set; }

    /// <summary>Дата создания.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>Внешний ключ владельца.</summary>
    public Guid OwnerId { get; set; }

    /// <summary>Навигационное свойство — владелец процесса.</summary>
    public User? Owner { get; set; }

    /// <summary>Навигационное свойство — задачи процесса.</summary>
    public ICollection<TaskItem> Tasks { get; set; } = [];
}
