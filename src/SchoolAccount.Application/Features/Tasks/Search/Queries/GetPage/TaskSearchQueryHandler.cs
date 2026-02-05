using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchQueryHandler(IPageReadStore pageReadStore)
    : IQueryHandler<TaskSearchQuery, TaskWithSubTasks>
{
    public async Task<Result<TaskWithSubTasks>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var result = await pageReadStore.SearchTasksAsync(query, cancellationToken);

        return Result.Success(result);
    }
}