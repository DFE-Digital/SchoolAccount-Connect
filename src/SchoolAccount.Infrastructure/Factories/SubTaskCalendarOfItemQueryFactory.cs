using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Infrastructure.Abstraction;
using SchoolAccount.Infrastructure.Projection;
using SchoolAccount.Infrastructure.Specifications;
using SchoolAccount.Kernel;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;

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

    public IQueryable<CalendarOfItemsRow> Query()
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        return applicationDbContext
            .SubTasks.AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.WorkflowState)
            .Include(x => x.TagsSourceMappings)
            .Where(SubTaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(SubTaskEntitySpecifications.IsVisible())
            .Where(SubTaskEntitySpecifications.IsMandatory())
            .Select(CalendarOfItemsRowProjection.FromSubTask());
    }
}
