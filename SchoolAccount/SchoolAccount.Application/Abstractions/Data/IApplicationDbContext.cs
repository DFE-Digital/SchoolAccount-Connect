using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Persistence.Teams;
using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<TeamDao> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}