using SchoolAccount.Application.Features.Shared.Query.Delegates;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;

namespace SchoolAccount.Application.Extensions;

public static class QueryableExtensions
{
    public static IList<T> WithSorting<T>(this IList<T> query, GenericOrderFunction<T>? customOrderBy = null)
        where T : IQueryRow
    {
        return customOrderBy is not null ? customOrderBy(query).ToList() : query.OrderBy(x => x.SortDate).ToList();
    }
}
