using SchoolAccount.Web.Connect.Models.Interfaces;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Models.Shared;

public record PaginationViewModel(IReadOnlyList<IPaginationItem> Items) : IPagedList
{
    public PaginationViewModel(bool showPagination)
        : this([])
    {
        _showPagination = showPagination;
    }

    private bool? _showPagination;

    public Uri? PreviousUrl { get; init; }
    public Uri? NextUrl { get; init; }

    public bool ShowPagination =>
        _showPagination ??= Items.OfType<PaginationItemViewModel>().Select(i => i.PageNumber).Distinct().Count() > 1;

    public bool ShowPrevious => PreviousUrl is not null;

    public bool ShowNext => NextUrl is not null;

    public int PageCount { get; set; }
    public int TotalItemCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool IsFirstPage { get; set; }
    public bool IsLastPage { get; set; }
    public int FirstItemOnPage { get; set; }
    public int LastItemOnPage { get; set; }
}
