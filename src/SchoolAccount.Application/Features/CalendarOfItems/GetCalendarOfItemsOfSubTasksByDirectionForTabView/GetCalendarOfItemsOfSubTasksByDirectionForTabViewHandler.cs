using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Factories;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;

public class GetCalendarOfItemsOfSubTasksByDirectionForTabViewHandler(
    IQueryAggregator aggregator,
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryHandler<GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery, GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public async Task<Result<GenericQueryPagedResult<CalendarOfItemsRow>>> Handle(
        GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery query,
        CancellationToken cancellationToken
    )
    {
        return await aggregator.Query(
            [new QueryFactoryOfSubTasksForCalendarOfItems(applicationDbContext, organisationContext)],
            [new SubTaskFilterableFactory(applicationDbContext)],
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

        return filter;
    }
}
