using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed class GetAllTasksQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetAllTasksQuery, GetAllTasksResponse>
{
    public async Task<Result<GetAllTasksResponse>> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(TaskEntitySpecifications.HasVisibleSubTasks())
            .AsSingleQuery()
            .Select(GetAllTasksProjection.ToGetAllTasksResponseTasks())
            .PaginateAsync(query.PageSize, query.PageNumber, cancellationToken);

        return Result.Success(new GetAllTasksResponse { Tasks = tasks });
    }
}
