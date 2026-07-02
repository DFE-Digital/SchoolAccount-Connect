using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Kernel;
using static SchoolAccount.Application.Specifications.SubTaskEntitySpecifications;
using static SchoolAccount.Application.Specifications.TypeEntitySpecifications;

namespace SchoolAccount.Application.Features.Dashboard;

public sealed class GetDashboardQueryHandler(
    IApplicationDbContext applicationDbContext,
    IOrganisationContext organisationContext,
    IDateTimeProvider dateTimeProvider
) : IQueryHandler<GetDashboardQuery, GetDashboardResponse>
{
    public async Task<Result<GetDashboardResponse>> Handle(GetDashboardQuery query, CancellationToken cancellationToken)
    {
        var rangeStart = query.Date.StartOfMonth();
        var rangeEnd = query.Date.AddMonths(12).EndOfMonth();

        var schoolTypeMappings = applicationDbContext.SchoolTypeTagMappings.AsQueryable();

        var calendarItems = await applicationDbContext
            .SubTasks.AsNoTracking()
            .Where(IsAccessibleForSchoolType(schoolTypeMappings, organisationContext.Type))
            .Where(IsPublished())
            .Where(HasDate())
            .Where(WithinDateRange(rangeStart, rangeEnd))
            .OrderBy(x => x.DueDate ?? x.StartDate)
            .Take(10)
            .Select(GetDashboardProjection.CalendarItem())
            .ToListAsync(cancellationToken);

        var categories = await applicationDbContext
            .Types.AsNoTracking()
            .Where(OnlyActiveHubTypes())
            .Where(TopLevelOnly())
            .Where(HasAssociatedTasks())
            .OrderBy(c => c.Name)
            .Select(GetDashboardProjection.Category())
            .ToListAsync(cancellationToken);

        var response = new GetDashboardResponse { CalendarOfItems = calendarItems, Categories = categories };
        var enricher = new GetDashboardResponseEnricher(dateTimeProvider);

        return Result.Success(enricher.Enrich(response));
    }
}
