using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Persistence.Teams;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<TeamDatabaseEntity> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}