using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchQueryHandler(IPageReadStore pageReadStore)
    : IQueryHandler<TaskSearchQuery, TaskWithSubTasksDto>
{
    public async Task<Result<TaskWithSubTasksDto>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var result = await pageReadStore.GetAllPagesAsync(new(query.Term), cancellationToken);
        return Result.Success(result);
    }
}
