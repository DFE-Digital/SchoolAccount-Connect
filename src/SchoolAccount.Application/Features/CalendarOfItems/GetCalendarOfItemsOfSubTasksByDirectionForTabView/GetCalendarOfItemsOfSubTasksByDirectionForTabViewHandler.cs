using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Features.Shared.Query.QueryFactories;
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

        var model = new CalendarOfItemsQueryCriteria
        {
            Range = DetermineDateRange(query),
            ViewModes = query.ViewModes,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = filter,
            CustomOrderByFunction = x => x.WithSorting(query.ViewModes, query.SortMode),
        };
        IEnumerable<IQueryFactory<CalendarOfItemsRow>> factories =
        [
            new SubTaskQueryFactory(applicationDbContext, organisationContext)
        ];

        return await aggregator.Query(factories, model, cancellationToken);
    }

    public static DateOnlyRange DetermineDateRange(GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery filter)
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
            rangeStart = filter.QueryFromDate.AddMonths(-filter.ViewPeriodInMonths).StartOfMonth();
            rangeEnd = filter.QueryFromDate.EndOfMonth();
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
