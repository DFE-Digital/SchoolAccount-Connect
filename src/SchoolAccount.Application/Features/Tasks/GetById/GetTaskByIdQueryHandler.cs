using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed class GetTaskByIdQueryHandler(
    IApplicationDbContext applicationDbContext,
    IDateTimeProvider dateTimeProvider,
    IOrganisationContext organisationContext
) : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    public async Task<Result<GetTaskByIdResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var accessibleTags = applicationDbContext.SchoolTypeTagMappings.AsQueryable();
        var task = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(TaskEntitySpecifications.IsAccessibleForSchoolType(accessibleTags, organisationContext.Type))
            .Where(task => task.Id == query.Id)
            .AsSingleQuery()
            .Select(GetTaskByIdProjection.ToTaskResponse(accessibleTags, organisationContext.Type))
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
        {
            return Result.Failure<GetTaskByIdResponse>(GetTaskByIdErrors.NotFound(query.Id));
        }

        var enricher = new GetTaskByIdResponseEnricher(dateTimeProvider);

        return Result.Success(enricher.Enrich(task));
    }
}
