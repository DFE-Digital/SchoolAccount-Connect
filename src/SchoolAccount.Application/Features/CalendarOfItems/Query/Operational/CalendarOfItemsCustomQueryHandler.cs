using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;

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
            CustomOrderByFunction = query.CustomOrderBy,
        };

        var result = await aggregator.Query(model, cancellationToken);

        if (result.IsFailure)
        {
            return result;
            //throw new ApplicationException();
        }

        return result;
    }
}
