using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Application.Features.Tasks.GetById;
using X.PagedList;

namespace SchoolAccount.Tests.Common.Builders.Tasks;

public class GetAllTasksResponseBuilder
{
    private List<GetAllTasksResponseTasks> _tasks = [];

    public static GetAllTasksResponseBuilder AResponse() => new();

    public GetAllTasksResponseBuilder WithTasks(params GetAllTasksResponseTasks[] tasks)
    {
        _tasks = [.. tasks];
        return this;
    }

    public GetAllTasksResponse Build() =>
        new(new StaticPagedList<GetAllTasksResponseTasks>(_tasks, 1, 10, _tasks.Count));

    public static implicit operator GetAllTasksResponse(GetAllTasksResponseBuilder builder) => builder.Build();
}
