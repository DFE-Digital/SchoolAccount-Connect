using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace SchoolAccount.Application.Extensions;

public static class PagedListExtensions
{
    public static IPagedList<T> ToStaticPagedList<T>(
        this IEnumerable<T> query,
        int pageNumber,
        int pageSize,
        int totalCount
    )
    {
        return new StaticPagedList<T>(query, pageNumber, pageSize, totalCount);
    }

    public static async Task<IPagedList<T>> PaginateAsync<T>(
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

    public static IPagedList<T> PaginateForExtraItem<T>(this IEnumerable<T> source, int pageSize, int pageNumber)
    {
        var list = source.ToList();
        var paginated = list.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
        return paginated.ToStaticPagedList(pageNumber, pageSize, list.Count);
    }
}
