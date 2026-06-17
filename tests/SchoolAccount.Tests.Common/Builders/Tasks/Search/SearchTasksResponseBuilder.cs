using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.Search;

namespace SchoolAccount.Tests.Common.Builders.Tasks.Search;

public class SearchTasksResponseBuilder
{
    private List<SearchTasksResponseTask> _tasks = [];

    public static SearchTasksResponseBuilder AResponse() => new();

    public SearchTasksResponseBuilder WithTasks(params SearchTasksResponseTask[] tasks)
    {
        _tasks = [.. tasks];
        return this;
    }

    private SearchTasksResponse Build() =>
        new()
        {
            Tasks = new PagedResult<SearchTasksResponseTask>
            {
                Items = _tasks,
                TotalCount = _tasks.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

    public static implicit operator SearchTasksResponse(SearchTasksResponseBuilder builder) => builder.Build();
}
