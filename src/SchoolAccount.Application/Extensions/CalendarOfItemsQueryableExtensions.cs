using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Delegates;

namespace SchoolAccount.Application.Extensions;

internal static class CalendarOfItemsQueryableExtensions
{
    internal static IOrderedQueryable<CalendarOfItemsRow> WithSorting(
        this IQueryable<CalendarOfItemsRow> query,
        CalendarOfItemsViewModes viewModes,
        CalendarOfItemsSortMode sortMode,
        GenericOrderFunction<CalendarOfItemsRow>? customOrderBy = null
    )
    {
        if (customOrderBy is not null)
        {
            return customOrderBy(query);
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
