using SchoolAccount.Domain.Common;
using X.PagedList;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public sealed record GetCategoryHubResponse(
    GetCategoryHubResponseCategory Category,
    IPagedList<GetCategoryHubResponseTasks> Tasks
);

public sealed record GetCategoryHubResponseCategory
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string? Description { get; init; }

    public string? HubViewDescription { get; init; }

    public GetCategoryHubResponseTypeGrouping? TypeGrouping { get; init; }

    public IReadOnlyCollection<GetCategoryHubResponseChildren> Children { get; init; } = [];
}

public sealed record GetCategoryHubResponseTypeGrouping
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public int? TypeLevel { get; init; }

    public bool? IsMandatory { get; init; }

    public bool? IsMultiSelect { get; init; }
}

public sealed record GetCategoryHubResponseChildren
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
}

public sealed record GetCategoryHubResponseTasks
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public Requirement? Requirement { get; init; }
}
