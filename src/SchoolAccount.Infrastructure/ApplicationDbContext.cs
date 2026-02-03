using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Infrastructure.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure;

internal sealed class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options
) : DbContext(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
