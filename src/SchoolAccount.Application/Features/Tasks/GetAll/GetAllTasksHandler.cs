using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed class GetAllTasksHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetAllTasksQuery, GetAllTasksResponse>
{
    public async Task<Result<GetAllTasksResponse>> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(TaskEntitySpecifications.IsVisible())
            .AsSingleQuery()
            .Select(GetAllTasksProjection.ToGetAllTasksResponseTasks())
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);

        return Result.Success(new GetAllTasksResponse(tasks));
    }
}
