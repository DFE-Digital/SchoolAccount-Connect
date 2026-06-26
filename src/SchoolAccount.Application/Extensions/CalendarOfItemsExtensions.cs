using SchoolAccount.Application.Features.CalendarOfItems.Common;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Interfaces;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Delegates;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Extensions;

public static class CalendarOfItemsExtensions
{
    internal static IOrderedEnumerable<CalendarOfItemsRow> WithSorting(
        this IList<CalendarOfItemsRow> query,
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

    public static DateOnlyRange DetermineDateRange(this ICalendarOfItemsDateQuery query)
    {
        var bothSet = CalendarOfItemsViewModes.Forward | CalendarOfItemsViewModes.Backward;
        if ((query.ViewModes & bothSet) == bothSet)
        {
            throw new ArgumentOutOfRangeException(
                nameof(query),
                query.ViewModes,
                "ViewModes cannot have both Forward and Backward set simultaneously."
            );
        }

        DateOnly rangeStart;
        DateOnly rangeEnd;

        if (query.ViewModes.HasFlag(CalendarOfItemsViewModes.Backward))
        {
            rangeStart = query.QueryFromDate.AddMonths(-query.ViewPeriodInMonths).StartOfMonth();
            rangeEnd = query.QueryFromDate.EndOfMonth();
        }
        else if (query.ViewModes.HasFlag(CalendarOfItemsViewModes.Forward))
        {
            rangeStart = query.QueryFromDate.StartOfMonth();
            rangeEnd = query.QueryFromDate.AddMonths(query.ViewPeriodInMonths).EndOfMonth();
        }
        else
        {
            throw new InvalidOperationException("Unsupported view mode");
        }

        return new DateOnlyRange(rangeStart, rangeEnd);
    }
}
