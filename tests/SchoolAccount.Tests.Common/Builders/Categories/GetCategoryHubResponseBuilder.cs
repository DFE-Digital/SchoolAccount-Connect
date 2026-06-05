using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using X.PagedList;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public class GetCategoryHubResponseBuilder
{
    private GetCategoryHubResponseCategory _category = GetCategoryHubResponseCategoryBuilder.AResponseCategory();
    private List<GetCategoryHubResponseTasks> _tasks = [];

    public static GetCategoryHubResponseBuilder AResponse() => new();

    public GetCategoryHubResponseBuilder WithCategory(GetCategoryHubResponseCategory category)
    {
        _category = category;
        return this;
    }

    public GetCategoryHubResponseBuilder WithTasks(params GetCategoryHubResponseTasks[] tasks)
    {
        _tasks = [.. tasks];
        return this;
    }

    public GetCategoryHubResponse Build() =>
        new(_category, new StaticPagedList<GetCategoryHubResponseTasks>(_tasks, 1, 10, _tasks.Count));

    public static implicit operator GetCategoryHubResponse(GetCategoryHubResponseBuilder builder) => builder.Build();
}
