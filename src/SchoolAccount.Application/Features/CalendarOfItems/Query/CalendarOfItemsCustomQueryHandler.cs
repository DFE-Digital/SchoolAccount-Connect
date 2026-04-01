using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public class CalendarOfItemsCustomQueryHandler(ICalendarOfItemsAggregator aggregator)
    : IQueryHandler<CalendarOfItemsCustomQuery, CalendarOfItemsPagedResult>
{
    public async Task<Result<CalendarOfItemsPagedResult>> Handle(
        CalendarOfItemsCustomQuery query,
        CancellationToken cancellationToken
    )
    {
        var model = new CalendarOfItemsCriteria
        {
            ToQuery = query.ToQuery,
            Range = query.QueryRange,
            ViewModes = CalendarOfItemsViewModes.Custom,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = query.Filter ?? new([]),
        };

        var result = await aggregator.Query(model, cancellationToken);

        return result;
    }
}
