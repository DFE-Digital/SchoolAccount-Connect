using SchoolAccount.Application.Abstractions.Infrastructure;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public class CalendarOfItemsDirectionalQueryHandler(ICalendarOfItemsAggregator aggregator)
    : IQueryHandler<CalendarOfItemsDirectionalQuery, CalendarOfItemsPagedResult>
{
    public async Task<Result<CalendarOfItemsPagedResult>> Handle(
        CalendarOfItemsDirectionalQuery query,
        CancellationToken cancellationToken
    )
    {
        var model = new CalendarOfItemsCriteria
        {
            ToQuery = query.ToQuery,
            Range = DetermineDateRange(query),
            ViewMode = CalendarOfItemsViewMode.Custom,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
        };

        return await aggregator.Query(model, cancellationToken);
    }

    private static DateOnlyRange DetermineDateRange(CalendarOfItemsDirectionalQuery filter)
    {
        DateOnly rangeStart;
        DateOnly rangeEnd;

        if (filter.ViewMode == CalendarOfItemsViewMode.Backward)
        {
            rangeStart = filter.QueryFromDate.AddMonths(-filter.ViewPeriodInMonths).AddMonths(-1).StartOfMonth();
            rangeEnd = filter.QueryFromDate.AddMonths(-1).EndOfMonth();
        }
        else if (filter.ViewMode == CalendarOfItemsViewMode.Forward)
        {
            rangeStart = filter.QueryFromDate.StartOfMonth();
            rangeEnd = filter.QueryFromDate.AddMonths(filter.ViewPeriodInMonths).EndOfMonth();
        }
        else
        {
            throw new InvalidOperationException("Unsupported view mode");
        }

        return new DateOnlyRange(rangeStart, rangeEnd);
    }
}
