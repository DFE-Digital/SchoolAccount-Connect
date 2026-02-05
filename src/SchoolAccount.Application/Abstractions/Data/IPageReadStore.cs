using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

namespace SchoolAccount.Application.Abstractions.Data;

public interface IPageReadStore
{
    Task<TaskWithSubTasks> SearchTasksAsync(
        TaskSearchQuery query,
        CancellationToken cancellationToken);
}