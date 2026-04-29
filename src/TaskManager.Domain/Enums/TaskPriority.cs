namespace TaskManager.Domain.Enums;

/// <summary>
/// Приоритет задачи.
/// </summary>
public enum TaskPriority
{
    /// <summary>Низкий приоритет.</summary>
    Low = 0,

    /// <summary>Средний приоритет.</summary>
    Medium = 1,

    /// <summary>Высокий приоритет.</summary>
    High = 2,

    /// <summary>Критический приоритет.</summary>
    Critical = 3
}
