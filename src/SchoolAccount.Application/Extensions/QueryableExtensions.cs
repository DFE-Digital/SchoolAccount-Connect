using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

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