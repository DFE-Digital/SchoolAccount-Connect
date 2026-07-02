using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.AllConstructors)]
public sealed class CalendarOfItemsQueryHandler(ICalendarOfItemsAggregator aggregator)
    : IQueryHandler<CalendarOfItemsQuery, CalendarOfItemsResponse>
{
    public async Task<Result<CalendarOfItemsResponse>> Handle(
        CalendarOfItemsQuery query,
        CancellationToken cancellationToken
    )
    {
        var filter = query.Filter;

        if (query.ViewModes.HasFlags(CalendarOfItemsViewModes.Forward, CalendarOfItemsViewModes.Backward))
        {
            filter.Add(
                new FilterRequest
                {
                    Field = "state",
                    Operator = ComparisonType.Equals,
                    Value = query.ViewModes switch
                    {
                        CalendarOfItemsViewModes.Backward => WorkflowState.Expired,
                        CalendarOfItemsViewModes.Forward => WorkflowState.Published,
                        _ => throw new ArgumentOutOfRangeException(
                            nameof(query),
                            query.ViewModes,
                            "View Mode incorrectly set"
                        ),
                    },
                }
            );
        }

        var model = new CalendarOfItemsCriteria
        {
            ToQuery = query.ToQuery,
            Range = DetermineDateRange(query.ViewModes, query.ViewPeriodInMonths, query.QueryFromDate),
            ViewModes = query.ViewModes,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = filter,
            CustomOrderByFunction = query.CustomOrderBy,
        };

        return await aggregator.Query(model, cancellationToken);
    }

    public static DateOnlyRange DetermineDateRange(
        CalendarOfItemsViewModes viewModes,
        int viewPeriodInMonths,
        DateOnly queryFromDate
    )
    {
        DateOnly rangeStart;
        DateOnly rangeEnd;

        if (viewModes.HasFlag(CalendarOfItemsViewModes.Backward))
        {
            rangeStart = queryFromDate.AddMonths(-viewPeriodInMonths).StartOfMonth();
            rangeEnd = queryFromDate.EndOfMonth();
        }
        else if (viewModes.HasFlag(CalendarOfItemsViewModes.Forward))
        {
            rangeStart = queryFromDate.StartOfMonth();
            rangeEnd = queryFromDate.AddMonths(viewPeriodInMonths).EndOfMonth();
        }
        else
        {
            throw new InvalidOperationException("Unsupported view mode");
        }

        return new DateOnlyRange(rangeStart, rangeEnd);
    }
}
