using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace SchoolAccount.Infrastructure.Extensions;

internal static class PagedListExtensions
{
    internal static IPagedList<T> ToStaticPagedList<T>(
        this IEnumerable<T> query,
        int pageNumber,
        int pageSize,
        int totalCount
    )
    {
        return new StaticPagedList<T>(query, pageNumber, pageSize, totalCount);
    }

    internal static async Task<IPagedList<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    )
    {
        var count = await query.CountAsync(cancellationToken);
        var paginated = await query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync(cancellationToken);
        return paginated.ToStaticPagedList(pageNumber, pageSize, count);
    }
}
