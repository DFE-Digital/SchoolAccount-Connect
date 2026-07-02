using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Categories.Handlers;

public class TestGetCategoryHubQueryHandler : IQueryHandler<GetCategoryHubQuery, GetCategoryHubResponse>
{
    private readonly List<GetCategoryHubResponseTasks> _tasks = [];
    private int _id = 2;
    private string _displayName = "Category name";

    public async Task<Result<GetCategoryHubResponse>> Handle(
        GetCategoryHubQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = new GetCategoryHubResponse
        {
            Id = _id,
            DisplayName = _displayName,
            Tasks = new PagedResult<GetCategoryHubResponseTasks>
            {
                Items = _tasks,
                TotalCount = _tasks.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

        return await Task.FromResult(Result.Success(response));
    }

    public TestGetCategoryHubQueryHandler WithId(int id)
    {
        _id = id;
        return this;
    }

    public TestGetCategoryHubQueryHandler WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public TestGetCategoryHubQueryHandler AddTask(GetCategoryHubResponseTasks task)
    {
        _tasks.Add(task);
        return this;
    }

    public void Clear()
    {
        _tasks.Clear();
        _id = 2;
        _displayName = "Category name";
    }
}
