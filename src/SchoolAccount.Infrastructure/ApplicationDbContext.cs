using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Infrastructure.Teams;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure;

internal sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider
) : DbContext(options), IApplicationDbContext
{
    public DbSet<TeamDatabaseEntity> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var now = dateTimeProvider.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableDatabaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userContext.UserId;
                    entry.Entity.DateCreated = now;
                    entry.Entity.UpdatedBy = userContext.UserId;
                    entry.Entity.DateUpdated = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userContext.UserId;
                    entry.Entity.DateUpdated = now;
                    break;
            }
        }

        //TODO: Reimplement emitted events code
        return await base.SaveChangesAsync(cancellationToken);
    }
}
