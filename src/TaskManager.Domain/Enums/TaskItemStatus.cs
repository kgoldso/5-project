namespace TaskManager.Domain.Enums;

/// <summary>
/// Статус выполнения задачи.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>Задача создана, но ещё не начата.</summary>
    NotStarted = 0,

    /// <summary>Задача в процессе выполнения.</summary>
    InProgress = 1,

    /// <summary>Задача завершена.</summary>
    Completed = 2,

    /// <summary>Задача отменена.</summary>
    Cancelled = 3
}
