using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Pipelines;
using SchoolAccount.Application.Pipelines.Query;
using SchoolAccount.Application.Pipelines.Filters;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;

public class GetCalendarOfItemsOfSubTasksByDirectionForTabViewHandler(
    IQueryAggregator aggregator,
    CalendarOfItemsPipeline calendarOfItemsPipeline
) : IQueryHandler<GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery, GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public async Task<Result<GenericQueryPagedResult<CalendarOfItemsRow>>> Handle(
        GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery query,
        CancellationToken cancellationToken
    )
    {
        return await aggregator.Query(
            calendarOfItemsPipeline.Query,
            calendarOfItemsPipeline.Filters,
            new CalendarOfItemsQueryCriteria
            {
                Range = query.DetermineDateRange(),
                ViewModes = query.ViewModes,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortMode = query.SortMode,
                Filter = BuildFilter(query),
                CustomOrderByFunction = x => x.WithSorting(query.ViewModes, query.SortMode),
            },
            cancellationToken
        );
    }

    private static IList<FilterRequest> BuildFilter(GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery query)
    {
        var filter = query.Filter ?? [];

        if (query.ViewModes.HasFlags(CalendarOfItemsViewModes.Forward, CalendarOfItemsViewModes.Backward))
        {
            filter.Add(
                new FilterRequest
                {
                    Field = "state",
                    Operator = ComparisonType.Equals,
                    // ReSharper disable once HeapView.BoxingAllocation
                    Value = query.ViewModes switch
                    {
                        var t when t.HasFlag(CalendarOfItemsViewModes.Backward) => WorkflowState.Expired,
                        var t when t.HasFlag(CalendarOfItemsViewModes.Forward) => WorkflowState.Published,
                        _ => throw new ArgumentOutOfRangeException(
                            nameof(query),
                            query.ViewModes,
                            "View Mode incorrectly set"
                        ),
                    },
                }
            );
        }

        return filter;
    }
}
