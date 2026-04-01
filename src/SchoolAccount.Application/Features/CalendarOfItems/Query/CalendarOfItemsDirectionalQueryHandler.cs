using SchoolAccount.Application.Abstractions.Aggregators;
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
            ViewModes = query.ViewModes,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = query.Filter ?? new([]),
        };

        return await aggregator.Query(model, cancellationToken);
    }

    public static DateOnlyRange DetermineDateRange(CalendarOfItemsDirectionalQuery filter)
    {
        var bothSet = CalendarOfItemsViewModes.Forward | CalendarOfItemsViewModes.Backward;
        if ((filter.ViewModes & bothSet) == bothSet)
        {
            throw new ArgumentOutOfRangeException(
                nameof(filter),
                filter.ViewModes,
                "ViewModes cannot have both Forward and Backward set simultaneously."
            );
        }

        DateOnly rangeStart;
        DateOnly rangeEnd;

        if (filter.ViewModes.HasFlag(CalendarOfItemsViewModes.Backward))
        {
            rangeStart = filter.QueryFromDate.AddMonths(-filter.ViewPeriodInMonths).AddMonths(-1).StartOfMonth();
            rangeEnd = filter.QueryFromDate.AddMonths(-1).EndOfMonth();
        }
        else if (filter.ViewModes.HasFlag(CalendarOfItemsViewModes.Forward))
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
