using SchoolAccount.Web.Connect.Features.Shared.Pagination;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryHub;

public sealed record CategoryHubViewModel
{
    public required int CategoryId { get; init; }

    public required string Name { get; init; }

    public string? HubViewDescription { get; init; }

    public required PaginatedListViewModel Tasks { get; init; }

    public bool IsAcademyTrustHandbookCategory => CategoryId == 1;
}
