using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<TaskSearchQuery, TaskSearchResponse>
{
    public async Task<Result<TaskSearchResponse>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {
        var term = query.Term?.Trim();
        var pageNumber = Math.Max(query.PageNumber, 1);
        var pageSize = Math.Max(query.PageSize, 1);

        if (string.IsNullOrWhiteSpace(term))
        {
            return Result.Success(
                new TaskSearchResponse(Enumerable.Empty<TaskListItem>().ToStaticPagedList(pageNumber, pageSize, 0))
            );
        }

        var like = $"%{term}%";

        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(t => EF.Functions.Like(t.Name, like) || EF.Functions.Like(t.Description ?? string.Empty, like))
            .OrderBy(t => t.Name)
            .Select(t => new TaskListItem(t.Id, t.Name, t.Description ?? string.Empty, t.DateUpdated))
            .PaginateAsync(pageSize, pageNumber, cancellationToken);

        return Result.Success(new TaskSearchResponse(tasks));
    }
}
