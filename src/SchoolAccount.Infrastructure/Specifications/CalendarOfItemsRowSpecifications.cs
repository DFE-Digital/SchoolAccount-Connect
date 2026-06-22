using System.Linq.Expressions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Specifications;

public static class CalendarOfItemsRowSpecifications
{
    public static Expression<Func<CalendarOfItemsRow, bool>> IsWithinDateRange(DateOnly rangeStart, DateOnly rangeEnd)
    {
        return x => x.SortDate.HasValue && x.SortDate.Value >= rangeStart && x.SortDate.Value <= rangeEnd;
    }

    public static Expression<Func<CalendarOfItemsRow, bool>> IsWithinDateRange(DateOnlyRange range)
    {
        return IsWithinDateRange(range.Start, range.End);
    }
}
