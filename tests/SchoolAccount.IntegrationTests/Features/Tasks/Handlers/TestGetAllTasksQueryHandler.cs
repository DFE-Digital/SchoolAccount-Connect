using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Tasks.Handlers;

public class TestGetAllTasksQueryHandler : IQueryHandler<GetAllTasksQuery, GetAllTasksResponse>
{
    private readonly List<GetAllTasksResponseTask> _tasks = [];

    public async Task<Result<GetAllTasksResponse>> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        var response = new GetAllTasksResponse
        {
            Tasks = new PagedResult<GetAllTasksResponseTask>
            {
                Items = _tasks,
                TotalCount = _tasks.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

        return await Task.FromResult(Result.Success(response));
    }

    public TestGetAllTasksQueryHandler AddTask(GetAllTasksResponseTask task)
    {
        _tasks.Add(task);
        return this;
    }

    public void Clear() => _tasks.Clear();
}
