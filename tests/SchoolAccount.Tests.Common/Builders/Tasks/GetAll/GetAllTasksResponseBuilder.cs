using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.GetAll;

namespace SchoolAccount.Tests.Common.Builders.Tasks.GetAll;

public class GetAllTasksResponseBuilder
{
    private List<GetAllTasksResponseTask> _tasks = [];

    public static GetAllTasksResponseBuilder AResponse() => new();

    public GetAllTasksResponseBuilder WithTasks(params GetAllTasksResponseTask[] tasks)
    {
        _tasks = [.. tasks];
        return this;
    }

    public GetAllTasksResponse Build() =>
        new()
        {
            Tasks = new PagedResult<GetAllTasksResponseTask>
            {
                Items = _tasks,
                TotalCount = _tasks.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

    public static implicit operator GetAllTasksResponse(GetAllTasksResponseBuilder builder) => builder.Build();
}
