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
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Factories.Query;

public class QueryFactoryOfTasksForCalendarOfItems(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext
) : IQueryFactory<CalendarOfItemsRow>
{
    public Type TypeBeingRegistered => typeof(TaskEntity);

    public async Task<QueryResponse<CalendarOfItemsRow>> Query(GenericQueryCriteria<CalendarOfItemsRow> criteria, FieldSelectorMapping mappings,
        CancellationToken cancellationToken)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        var query = applicationDbContext
            .Tasks.AsNoTracking()
            .Include(x => x.SubTasks)
            .ThenInclude(x => x.TagsSourceMappings)
            .Include(x => x.TypeTaskMappings)
            .Where(TaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(TaskEntitySpecifications.IsVisible())
            .Apply(criteria.Filter, mappings)
            .Select(CalendarOfItemsRowProjection.FromTask());
        
        return (
            await query.CountAsync(cancellationToken), 
            await query.ToListAsync(cancellationToken));
    }
}
