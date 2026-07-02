using SchoolAccount.Web.Connect.Features.Shared.Pagination;

namespace SchoolAccount.Web.Connect.Features.Tasks.Search;

public record SearchTasksViewModel
{
    public required string SearchTerm { get; init; }

    public required PaginatedListViewModel Tasks { get; init; }

    public string Heading => "Search results";

    public required string Description { get; init; }

    public required string SubHeading { get; init; }
}
