using SchoolAccount.Application.Features.Shared.Query.Delegates;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> WithSorting<T>(this IQueryable<T> query, GenericOrderFunction<T>? customOrderBy = null)
    where T : IQueryRow
    {
        return customOrderBy is not null 
            ? customOrderBy(query) 
            : query.OrderBy(x => x.SortDate);
    }
}