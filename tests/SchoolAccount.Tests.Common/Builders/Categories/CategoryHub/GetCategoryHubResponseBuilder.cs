using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;

namespace SchoolAccount.Tests.Common.Builders.Categories.CategoryHub;

public class GetCategoryHubResponseBuilder
{
    private int _id = 1;
    private string _displayName = "Category name";
    private string? _hubViewDescription;
    private List<GetCategoryHubResponseTasks> _tasks = [];

    public static GetCategoryHubResponseBuilder AResponse() => new();

    public GetCategoryHubResponseBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GetCategoryHubResponseBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public GetCategoryHubResponseBuilder WithTasks(params GetCategoryHubResponseTasks[] tasks)
    {
        _tasks = [.. tasks];
        return this;
    }

    public GetCategoryHubResponseBuilder WithHubViewDescription(string hubViewDescription)
    {
        _hubViewDescription = hubViewDescription;
        return this;
    }

    public GetCategoryHubResponse Build() =>
        new()
        {
            Id = _id,
            DisplayName = _displayName,
            HubViewDescription = _hubViewDescription,
            Tasks = new PagedResult<GetCategoryHubResponseTasks>
            {
                Items = _tasks,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = _tasks.Count,
            },
        };

    public static implicit operator GetCategoryHubResponse(GetCategoryHubResponseBuilder builder) => builder.Build();
}
