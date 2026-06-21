using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Infrastructure.Abstraction;
using SchoolAccount.Infrastructure.Helpers.Filtering;
using SchoolAccount.Infrastructure.Projection;
using SchoolAccount.Infrastructure.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Factories;

public class TaskCalendarOfItemQueryFactory(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : ICalendarOfItemsQueryFactory
{
    public bool IsQueryableFor(CalendarOfItemsQueryTypes identifier)
    {
        return (identifier & CalendarOfItemsQueryTypes.Task) == CalendarOfItemsQueryTypes.Task;
    }

    public IQueryable<CalendarOfItemsRow> Query(CalendarOfItemsFilter filter, FieldSelectorMapping mappings)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        return applicationDbContext
            .Tasks.AsNoTracking()
            .Include(x => x.SubTasks)
                .ThenInclude(x => x.TagsSourceMappings)
            .Include(x => x.Types)
            .Where(TaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(TaskEntitySpecifications.IsVisible())
            .Apply(filter, mappings)
            .Select(CalendarOfItemsRowProjection.FromTask());
    }
}
