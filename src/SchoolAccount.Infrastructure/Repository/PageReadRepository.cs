using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

namespace SchoolAccount.Infrastructure.Repository;

public sealed class PageReadRepository(IApplicationDbContext applicationDbContext) : IPageReadStore
{
    public async Task<TaskWithSubTasks> SearchTasksAsync(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var term = query.Term?.Trim();

        if (string.IsNullOrWhiteSpace(term))
        {
            return new TaskWithSubTasks(Array.Empty<TaskListItem>(), Array.Empty<SubTaskListItem>());
        }

        var tasksQuery = applicationDbContext.Tasks
            .AsNoTracking()
            .Where(x => x.IsDeleted != true)
            .Where(x => x.IsLatestVersion);

        if (!string.IsNullOrWhiteSpace(term))
        {
            var like = $"%{term}%";
            tasksQuery = tasksQuery.Where(x =>
                EF.Functions.Like(x.TaskName!, like) ||
                EF.Functions.Like(x.TaskReferenceNo!, like));
        }

        var tasks = await tasksQuery
            .OrderByDescending(x => x.DateUpdated)
            .Select(x => new TaskListItem(
                x.Id,
                x.TaskReferenceNo ?? string.Empty,
                x.TaskName ?? string.Empty,
                x.UpdatedBy,
                x.DateUpdated
            ))
            .ToListAsync(cancellationToken);

        var taskIds = tasks.Select(x => x.Id).ToArray();

        var subTasks = await applicationDbContext.SubTasks
            .AsNoTracking()
            .Where(x => x.IsDeleted != true)
            .Where(x => taskIds.Contains(x.TaskId))
            .OrderByDescending(x => x.DateUpdated)
            .Select(x => new SubTaskListItem(
                x.Id,
                x.SubTaskName ?? x.SubTaskReferenceNo ?? string.Empty,
                x.UpdatedBy,
                x.DateUpdated
            ))
            .ToListAsync(cancellationToken);

        return new TaskWithSubTasks(tasks, subTasks);
    }
}