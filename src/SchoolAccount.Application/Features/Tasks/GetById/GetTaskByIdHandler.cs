using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed class GetTaskByIdHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
    : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    public async Task<Result<GetTaskByIdResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var task = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(task => task.Id == query.Id)
            .AsSingleQuery()
            .Select(GetTaskByIdProjection.ToTaskResponse())
            .FirstOrDefaultAsync(cancellationToken);

        if (task is null)
        {
            return Result.Failure<GetTaskByIdResponse>(GetTaskByIdErrors.NotFound(query.Id));
        }

        var enricher = new GetTaskByIdResponseEnricher(dateTimeProvider);

        return Result.Success(enricher.Enrich(task));
    }
}
