using SchoolAccount.Web.Connect.Models.Interfaces;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Models;

public record PaginationViewModel(IReadOnlyList<IPaginationItem> Items) : IPagedList
{
    private bool? _showPagination;

    public string? PreviousUrl { get; init; }
    public string? NextUrl { get; init; }

    public bool ShowPagination =>
        _showPagination ??= Items.OfType<PaginationItemViewModel>().Select(i => i.PageNumber).Distinct().Count() > 1;

    public bool ShowPrevious => !string.IsNullOrEmpty(PreviousUrl);

    public bool ShowNext => !string.IsNullOrEmpty(NextUrl);

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
