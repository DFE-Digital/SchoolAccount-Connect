using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries;

public sealed class TaskSearchQueryHandler()
    : IQueryHandler<TaskSearchQuery, TaskSearchResultsVm>
{
    public Task<Result<TaskSearchResultsVm>> Handle(TaskSearchQuery query, CancellationToken cancellationToken)
    {

        return Task.FromResult((Result<TaskSearchResultsVm>)Result.Success());
    }
}