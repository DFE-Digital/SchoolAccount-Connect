using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Infrastructure.Extensions;

internal static class CalendarOfItemsQueryableExtensions
{
    internal static IQueryable<CalendarOfItemsRow> WithSorting(
        this IQueryable<CalendarOfItemsRow> query,
        CalendarOfItemsViewMode viewMode,
        CalendarOfItemsSortMode sortMode
    )
    {
        return viewMode switch
        {
            CalendarOfItemsViewMode.Backward => sortMode switch
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
