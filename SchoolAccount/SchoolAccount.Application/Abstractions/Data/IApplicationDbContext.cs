using Microsoft.EntityFrameworkCore;
using SchoolAccount.Domain.Teams;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Team> Teams { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}