using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search;

public sealed class SearchTasksQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<SearchTasksQuery, SearchTasksResponse>
{
    public async Task<Result<SearchTasksResponse>> Handle(SearchTasksQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.Term))
        {
            var emptyResult = new PagedResult<SearchTasksResponseTask>();
            var emptyResponse = new SearchTasksResponse { Tasks = emptyResult };

            return Result.Success(emptyResponse);
        }

        var escaped = EscapeLikeTerm(query.Term);
        var like = $"%{escaped}%";

        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(TaskEntitySpecifications.ContainsTerm(like))
            .OrderBy(t => t.Name)
            .Select(SearchTasksProjection.ToSearchTasksResponseTask())
            .PaginateAsync(query.PageSize, query.PageNumber, cancellationToken);

        return Result.Success(new SearchTasksResponse { Tasks = tasks });
    }

    private static string EscapeLikeTerm(string term) =>
        term.Replace("\\", @"\\", StringComparison.Ordinal)
            .Replace("%", "\\%", StringComparison.Ordinal)
            .Replace("_", "\\_", StringComparison.Ordinal);
}
