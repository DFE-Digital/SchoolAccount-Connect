using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchQueryHandler(
    IApplicationDbContext applicationDbContext,
    IDateTimeProvider dateTimeProvider
) : IQueryHandler<TaskSearchQuery, TaskSearchResponse>
{
    public async Task<Result<TaskSearchResponse>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var term = query.Term?.Trim();

        if (string.IsNullOrWhiteSpace(term))
        {
            return Result.Success(new TaskSearchResponse([]));
        }

        var to = dateTimeProvider.UtcNow.Date.AddMonths(12).ToDateOnly();
        var from = to.AddMonths(-12);
        var like = $"%{term}%";

        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(t =>
                EF.Functions.Like(t.Name, like)
                || EF.Functions.Like(t.Description ?? string.Empty, like)
            )
            .Where(t =>
                applicationDbContext.SubTasks.Any(st =>
                    st.TaskId == t.Id
                    && st.WorkflowState == WorkflowState.Published
                )
            )
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