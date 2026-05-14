using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchQueryHandler(
    IApplicationDbContext applicationDbContext,
    IDateTimeProvider dateTimeProvider
) : IQueryHandler<TaskSearchQuery, TaskSearchResponse>
{
    public async Task<Result<TaskSearchResponse>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var term = query.Term.Trim();
        var isInitialLoad = string.IsNullOrWhiteSpace(term);

        var from = dateTimeProvider.UtcNow.Date.ToDateOnly();
        var to = from.AddMonths(12);

        var tasksQuery = applicationDbContext
            .Tasks.AsNoTracking()
            .Where(t => t.IsDeleted != true)
            .Where(t => t.IsLatestVersion);

        if (isInitialLoad)
        {
            tasksQuery = tasksQuery.Where(t =>
                applicationDbContext.SubTasks.Any(st =>
                    st.IsDeleted != true
                    && st.TaskId == t.Id
                    && st.DueDate != null
                    && st.DueDate >= from
                    && st.DueDate < to
                )
            );
        }
        else
        {
            var like = $"%{term}%";
            tasksQuery = tasksQuery.Where(t =>
                EF.Functions.Like(t.Name, like) || EF.Functions.Like(t.Description!, like)
            );
        }

        var tasks = await tasksQuery
            .OrderByDescending(t => t.DateUpdated)
            .Select(t => new TaskListItem(
                t.Id,
                t.Name,
                t.Description ?? string.Empty,
                t.DateUpdated
            ))
            .ToListAsync(cancellationToken);
        

        return Result.Success(new TaskSearchResponse(tasks));
    }
}
