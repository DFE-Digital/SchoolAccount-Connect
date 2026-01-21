using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure;

internal sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options,
    IUserContext userContext,
    IDateTimeProvider dateTimeProvider
) : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrEmpty(userContext.EmailAddress);
        
        var now = dateTimeProvider.UtcNow;

        foreach (var entry in ChangeTracker.Entries<AuditableDatabaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userContext.EmailAddress;
                    entry.Entity.DateCreated = now;
                    entry.Entity.UpdatedBy = userContext.EmailAddress;
                    entry.Entity.DateUpdated = now;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedBy = userContext.EmailAddress;
                    entry.Entity.DateUpdated = now;
                    break;
            }
        }

        //TODO: Reimplement emitted events code
        return await base.SaveChangesAsync(cancellationToken);
    }
}
