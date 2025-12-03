using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Teams;

namespace SchoolAccount.Infrastructure;

internal class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options),
        IApplicationDbContext
{
    public DbSet<TeamDatabaseEntity> Teams { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        //TODO: Reimplement emitted events code
        return await base.SaveChangesAsync(cancellationToken);
    }
}
