using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Infrastructure;

namespace SchoolAccount.Tests.Common.Fakes;

public static class DatabaseContext
{
    public static IApplicationDbContext Build(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);
        return context;
    }

    public static async Task<IApplicationDbContext> Map(this IApplicationDbContext context, Action<IApplicationDbContext> mapper, 
        CancellationToken cancellationToken = default)
    {
        mapper(context);
        await context.SaveChangesAsync(cancellationToken);
        return context;
    }
}