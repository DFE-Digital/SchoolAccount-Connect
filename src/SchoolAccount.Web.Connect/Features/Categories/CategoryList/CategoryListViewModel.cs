using SchoolAccount.Web.Connect.Features.Shared.Pagination;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryList;

public sealed record CategoryListViewModel
{
    public required PaginatedListViewModel Categories { get; init; }

    public string Title => "Explore categories";

    public string Description => "View required tasks and optional guidance by category.";
}
