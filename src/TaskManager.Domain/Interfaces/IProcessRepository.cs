using TaskManager.Domain.Entities;

namespace TaskManager.Domain.Interfaces;

/// <summary>
/// Репозиторий для работы с процессами.
/// </summary>
public interface IProcessRepository : IRepository<Process>
{
    /// <summary>Получить все процессы конкретного владельца.</summary>
    Task<IEnumerable<Process>> GetAllByOwnerAsync(Guid ownerId);

    /// <summary>Получить процесс с его задачами.</summary>
    Task<Process?> GetWithTasksAsync(Guid processId);
}
