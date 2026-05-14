using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Infrastructure.Abstraction;
using SchoolAccount.Infrastructure.Helpers.Filtering;
using SchoolAccount.Infrastructure.Projection;
using SchoolAccount.Infrastructure.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Factories;

public class SubTaskCalendarOfItemQueryFactory(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : ICalendarOfItemsQueryFactory
{
    public bool IsQueryableFor(CalendarOfItemsQueryTypes identifier)
    {
        return (identifier & CalendarOfItemsQueryTypes.SubTask) == CalendarOfItemsQueryTypes.SubTask;
    }

    public IQueryable<CalendarOfItemsRow> Query(CalendarOfItemsFilter filter, FieldSelectorMapping mappings)
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
