using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
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

    public async Task<QueryResponse<CalendarOfItemsRow>> Query(GenericQueryCriteria<CalendarOfItemsRow> criteria, FieldSelectorMapping mappings,
        CancellationToken cancellationToken)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        var query = applicationDbContext
            .SubTasks.AsNoTracking()
            .Include(x => x.Task)
            .Include(x => x.TagsSourceMappings)
            .Where(SubTaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Apply(criteria.Filter, mappings)
            .Select(CalendarOfItemsRowProjection.FromSubTask());
        
        return (
            await query.CountAsync(cancellationToken), 
            await query.ToListAsync(cancellationToken));
    }
}
