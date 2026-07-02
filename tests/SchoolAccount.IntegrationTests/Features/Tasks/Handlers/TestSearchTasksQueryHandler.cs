using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.Search;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Tasks.Handlers;

public class TestSearchTasksQueryHandler : IQueryHandler<SearchTasksQuery, SearchTasksResponse>
{
    private readonly List<SearchTasksResponseTask> _tasks = [];

    public async Task<Result<SearchTasksResponse>> Handle(SearchTasksQuery query, CancellationToken cancellationToken)
    {
        var response = new SearchTasksResponse
        {
            Tasks = new PagedResult<SearchTasksResponseTask>
            {
                Items = _tasks,
                TotalCount = _tasks.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

        return await Task.FromResult(Result.Success(response));
    }

    public TestSearchTasksQueryHandler AddTask(SearchTasksResponseTask task)
    {
        _tasks.Add(task);
        return this;
    }

    public void Clear() => _tasks.Clear();
}
