using SchoolAccount.Application.Features.Calendars.CalendarList.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarList.Models;

namespace SchoolAccount.Infrastructure.Extensions;

internal static class CalendarOfItemsQueryableExtensions
{
    internal static IQueryable<CalendarOfItemsRow> WithSorting(
        this IQueryable<CalendarOfItemsRow> query,
        CalendarOfItemsViewModes viewModes,
        CalendarOfItemsSortMode sortMode,
        CalendarOfItemsOrderFunction? customOrderBy = null
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
