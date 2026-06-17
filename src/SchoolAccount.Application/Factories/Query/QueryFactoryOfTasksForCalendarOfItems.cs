using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Projections;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Factories.Query;

public class QueryFactoryOfTasksForCalendarOfItems(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryFactory<CalendarOfItemsRow>
{
    public Type TypeBeingRegistered => typeof(TaskEntity);

    public IQueryable<CalendarOfItemsRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        return applicationDbContext
            .Tasks.AsNoTracking()
            .Include(x => x.SubTasks)
                .ThenInclude(x => x.TagsSourceMappings)
            .Include(x => x.TypeTaskMappings)
            .Where(TaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(TaskEntitySpecifications.IsVisible())
            .Apply(filter, mappings)
            .Select(CalendarOfItemsRowProjection.FromTask());
    }
}
