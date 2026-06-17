using SchoolAccount.Application.Common;

namespace SchoolAccount.Application.Features.Categories.GetParentCategories;

public sealed record GetParentCategoriesResponse
{
    public required PagedResult<GetParentCategoriesResponseCategory> Categories { get; init; }
}

public sealed record GetParentCategoriesResponseCategory
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public string? Description { get; init; }

    public IReadOnlyCollection<GetParentCategoriesResponseChildren> Children { get; init; } = [];
}

public sealed record GetParentCategoriesResponseChildren
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;
}
