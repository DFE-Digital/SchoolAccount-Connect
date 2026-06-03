using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Features.Shared.Query.QueryFactories;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query.GetCalendarOfItemsOfNextTenItemsOfSubTasks;

public class GetCalendarOfItemsOfNextTenItemsOfSubTasksHandler(
    IQueryAggregator aggregator,
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryHandler<GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery, QueryPagedResult<CalendarOfItemsRow>>
{
    public async Task<Result<QueryPagedResult<CalendarOfItemsRow>>> Handle(
        GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery query,
        CancellationToken cancellationToken
    )
    {
        var model = new CalendarOfItemsQueryCriteria
        {
            Range = query.QueryRange,
            ViewModes = CalendarOfItemsViewModes.Custom,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = query.Filter ?? [],
            CustomOrderByFunction = x => x.WithSorting(CalendarOfItemsViewModes.Custom, query.SortMode)
        };
        IEnumerable<IQueryFactory<CalendarOfItemsRow>> factories =
        [
            new SubTaskQueryFactory(applicationDbContext, organisationContext)
        ];

        return await aggregator.Query(factories, model, cancellationToken);
    }
}
