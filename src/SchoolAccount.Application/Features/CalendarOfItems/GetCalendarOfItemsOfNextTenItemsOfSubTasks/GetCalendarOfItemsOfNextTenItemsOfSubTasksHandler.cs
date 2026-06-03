using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Factories;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfNextTenItemsOfSubTasks;

public class GetCalendarOfItemsOfNextTenItemsOfSubTasksHandler(
    IQueryAggregator aggregator,
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryHandler<GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery, GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public async Task<Result<GenericQueryPagedResult<CalendarOfItemsRow>>> Handle(
        GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery query,
        CancellationToken cancellationToken
    )
    {
        return await aggregator.Query(
            [
                new QueryFactoryOfSubTasksForCalendarOfItems(applicationDbContext, organisationContext)
            ], 
            [],
            new CalendarOfItemsQueryCriteria
            {
                Range = query.QueryRange,
                ViewModes = CalendarOfItemsViewModes.Custom,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortMode = query.SortMode,
                Filter = query.Filter ?? [],
                CustomOrderByFunction = x => x.WithSorting(CalendarOfItemsViewModes.Custom, query.SortMode)
            }, 
            cancellationToken);
    }
}
