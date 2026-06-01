using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;

namespace SchoolAccount.Infrastructure.Extensions;

internal static class CalendarOfItemsQueryableExtensions
{
    internal static IQueryable<CalendarOfItemsRow> WithSorting(
        this IQueryable<CalendarOfItemsRow> query,
        CalendarOfItemsViewModes viewModes,
        CalendarOfItemsSortMode sortMode,
        GenericOrderFunction<QueryRow>? customOrderBy = null
    )
    {
        if (customOrderBy is not null)
        {
            return (IQueryable<CalendarOfItemsRow>)customOrderBy(query);
        }

        return viewModes switch
        {
            CalendarOfItemsViewModes.Backward => sortMode switch
            {
                _ => query.OrderByDescending(x => x.SortDate),
            },
            _ => sortMode switch
            {
                _ => query.OrderBy(x => x.SortDate),
            },
        };
    }
}
