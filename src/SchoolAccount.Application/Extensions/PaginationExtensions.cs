using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Common;

namespace SchoolAccount.Application.Extensions;

public static class PaginationExtensions
{
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
