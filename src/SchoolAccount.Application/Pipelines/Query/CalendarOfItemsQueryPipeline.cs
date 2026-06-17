using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Pipelines;
using SchoolAccount.Application.Factories.Query;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Pipelines.Query;

public class CalendarOfItemsQueryPipeline(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryFactoryPipeline<CalendarOfItemsRow>
{
    public IList<IQueryFactory<CalendarOfItemsRow>> Factories { get; } =
    [
        new QueryFactoryOfSubTasksForCalendarOfItems(applicationDbContext, organisationContext)
    ];
}