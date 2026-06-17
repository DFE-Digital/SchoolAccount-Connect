using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Common;
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

    //[Obsolete("Use the new PaginateAsync method")]
    public static async Task<IPagedList<T>> PaginateAsyncOld<T>(
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

    public static async Task<PagedResult<T>> PaginateAsync<T>(
        this IQueryable<T> query,
        int pageSize,
        int pageNumber,
        CancellationToken cancellationToken = default
    )
    {
        var count = await query.CountAsync(cancellationToken);
        var paginated = await query.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync(cancellationToken);
        return new PagedResult<T>
        {
            Items = paginated,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = count,
        };
    }
}
