using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Repository;

public sealed class PageReadRepository(
    IApplicationDbContext applicationDbContext,
    IDateTimeProvider dateTimeProvider) : IPageReadStore
{
    public async Task<TaskWithSubTasks> GetAllPagesAsync(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var term = query.Term?.Trim();
        var isInitialLoad = string.IsNullOrWhiteSpace(term);

        var from = dateTimeProvider.UtcNow.Date;
        var to = from.AddMonths(12);

        var tasksQuery = applicationDbContext.Tasks
            .AsNoTracking()
            .Where(t => t.IsDeleted != true)
            .Where(t => t.IsLatestVersion);

        if (isInitialLoad)
        {
            tasksQuery = tasksQuery.Where(t =>
                applicationDbContext.SubTasks.Any(st =>
                    st.IsDeleted != true &&
                    st.TaskId == t.Id &&
                    st.DueDate != null &&
                    st.DueDate >= from &&
                    st.DueDate < to));
        }
        else
        {
            var like = $"%{term}%";
            tasksQuery = tasksQuery.Where(t =>
                EF.Functions.Like(t.Name!, like) ||
                EF.Functions.Like(t.ReferenceNo!, like));
        }

        var tasks = await tasksQuery
            .OrderByDescending(t => t.DateUpdated)
            .Select(t => new TaskListItem(
                t.Id,
                t.ReferenceNo ?? string.Empty,
                t.Name ?? string.Empty,
                t.UpdatedBy,
                t.DateUpdated
            ))
            .ToListAsync(cancellationToken);

        var taskIds = tasks.Select(t => t.Id).ToArray();

        var subTasksQuery = applicationDbContext.SubTasks
            .AsNoTracking()
            .Where(st => st.IsDeleted != true)
            .Where(st => taskIds.Contains(st.TaskId));

        if (isInitialLoad)
        {
            subTasksQuery = subTasksQuery
                .Where(st => st.DueDate != null)
                .Where(st => st.DueDate >= from && st.DueDate < to);
        }

        var subTasks = await subTasksQuery
            .OrderByDescending(st => st.DateUpdated)
            .Select(st => new SubTaskListItem(
                st.Id,
                st.Name ?? st.ReferenceNo ?? string.Empty,
                st.UpdatedBy,
                st.DateUpdated
            ))
            .ToListAsync(cancellationToken);

        return new TaskWithSubTasks(tasks, subTasks);
    }
}