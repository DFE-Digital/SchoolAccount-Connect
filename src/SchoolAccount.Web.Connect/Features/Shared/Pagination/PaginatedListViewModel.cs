using SchoolAccount.Web.Connect.Features.Shared.List;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Features.Shared.Pagination;

public record PaginatedListViewModel(
    IPagedList<ListItemViewModel> PaginatedItems,
    string NoResultsMessage = "No results found"
);
