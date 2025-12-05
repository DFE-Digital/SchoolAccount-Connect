using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure;

public interface IApplicationDbContext
{
    DbSet<TeamDatabaseEntity> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
