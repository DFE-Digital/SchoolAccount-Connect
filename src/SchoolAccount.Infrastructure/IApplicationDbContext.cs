using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Models;

namespace SchoolAccount.Infrastructure;

public interface IApplicationDbContext
{
    DbSet<TaskEntity> Tasks { get; }
    DbSet<SubTaskEntity> SubTasks { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
