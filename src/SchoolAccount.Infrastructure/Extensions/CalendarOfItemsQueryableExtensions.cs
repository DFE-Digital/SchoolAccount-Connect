using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Infrastructure.Extensions;

internal static class CalendarOfItemsQueryableExtensions
{
    internal static IQueryable<CalendarOfItemsRow> WithSorting(
        this IQueryable<CalendarOfItemsRow> query,
        CalendarOfItemsViewModes viewModes,
        CalendarOfItemsSortMode sortMode
    )
    {
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
