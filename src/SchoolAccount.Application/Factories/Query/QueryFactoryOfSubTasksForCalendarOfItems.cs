using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Interfaces;
using SchoolAccount.Application.Projections;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Factories.Query;

public class QueryFactoryOfSubTasksForCalendarOfItems(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryFactory<CalendarOfItemsRow>
{
    public Type TypeBeingRegistered => typeof(SubTaskEntity);

    public IQueryable<CalendarOfItemsRow> Query(IList<FilterRequest> filter, FieldSelectorMapping mappings)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        return applicationDbContext
            .SubTasks.AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.TagsSourceMappings)
            .Where(SubTaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Apply(filter, mappings)
            .Select(CalendarOfItemsRowProjection.FromSubTask());
    }
}
