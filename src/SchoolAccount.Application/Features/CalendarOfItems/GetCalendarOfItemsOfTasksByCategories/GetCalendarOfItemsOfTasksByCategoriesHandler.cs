using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Factories;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfTasksByCategories;

public class GetCalendarOfItemsOfTasksByCategoriesHandler(
    IQueryAggregator aggregator,
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryHandler<GetCalendarOfItemsOfTasksByCategoriesQuery, GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public async Task<Result<GenericQueryPagedResult<CalendarOfItemsRow>>> Handle(
        GetCalendarOfItemsOfTasksByCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        return await aggregator.Query(
            [new QueryFactoryOfTasksForCalendarOfItems(applicationDbContext, organisationContext)],
            [],
            new CalendarOfItemsQueryCriteria
            {
                Range = query.QueryRange,
                ViewModes = CalendarOfItemsViewModes.Custom,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SortMode = query.SortMode,
                Filter = query.Filter ?? [],
                CustomOrderByFunction = x => x.OrderBy(o => o.Name),
            },
            cancellationToken
        );
    }
}
