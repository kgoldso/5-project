using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Data;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Interfaces;

namespace TaskManager.Domain.Repositories;

/// <summary>
/// Реализация репозитория процессов.
/// </summary>
public class ProcessRepository(AppDbContext context) : Repository<Process>(context), IProcessRepository
{
    public async Task<IEnumerable<Process>> GetAllByOwnerAsync(Guid ownerId)
        => await DbSet
            .Where(p => p.OwnerId == ownerId)
            .Include(p => p.Tasks)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

    public async Task<Process?> GetWithTasksAsync(Guid processId)
        => await DbSet
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == processId);
}
