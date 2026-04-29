using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Data;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Реализация репозитория задач.
/// </summary>
public class TaskRepository(AppDbContext context) : Repository<TaskItem>(context), ITaskRepository
{
    public async Task<IEnumerable<TaskItem>> GetByProcessIdAsync(Guid processId)
        => await DbSet
            .Where(t => t.ProcessId == processId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
}
