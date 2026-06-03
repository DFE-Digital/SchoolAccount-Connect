using SchoolAccount.Application.Abstractions.Aggregators;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Contracts;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Features.Shared.Query.QueryFactories;
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
        var model = new CalendarOfItemsQueryCriteria
        {
            Range = query.QueryRange,
            ViewModes = CalendarOfItemsViewModes.Custom,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            SortMode = query.SortMode,
            Filter = query.Filter ?? [],
            CustomOrderByFunction = query.CustomOrderBy,
        };
        IEnumerable<IQueryFactory<CalendarOfItemsRow>> factories =
        [
            new SubTaskQueryFactory(applicationDbContext, organisationContext)
        ];

        return await aggregator.Query(factories, model, cancellationToken);
    }
}
