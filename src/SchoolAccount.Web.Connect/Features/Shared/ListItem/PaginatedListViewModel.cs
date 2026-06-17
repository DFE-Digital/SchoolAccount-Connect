using X.PagedList;

namespace SchoolAccount.Web.Connect.Features.Shared.ListItem;

public record PaginatedListViewModel(
    IPagedList<ListItemViewModel> PaginatedItems,
    string NoResultsMessage = "No results found"
);
