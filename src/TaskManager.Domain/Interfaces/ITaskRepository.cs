using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с задачами.
/// </summary>
public interface ITaskRepository : IRepository<TaskItem>
{
    /// <summary>Получить все задачи указанного процесса.</summary>
    Task<IEnumerable<TaskItem>> GetByProcessIdAsync(Guid processId);
}
