using X.PagedList;

namespace SchoolAccount.Web.Connect.Models.Shared;

public class Pagination : IPagedList
{
    private int? _pageNumber;

    public Pagination() { }

    public Pagination(IPagedList pagination)
    {
        PageCount = pagination.PageCount;
        TotalItemCount = pagination.TotalItemCount;
        PageNumber = pagination.PageNumber;
        PageSize = pagination.PageSize;
        HasPreviousPage = pagination.HasPreviousPage;
        HasNextPage = pagination.HasNextPage;
        IsFirstPage = pagination.IsFirstPage;
        IsLastPage = pagination.IsLastPage;
        FirstItemOnPage = pagination.FirstItemOnPage;
        LastItemOnPage = pagination.LastItemOnPage;
    }

    public int PageCount { get; init; }
    public int TotalItemCount { get; init; }

    public int PageNumber
    {
        get => _pageNumber ?? 1;
        set => _pageNumber = value;
    }
    public int PageSize { get; init; }
    public bool HasPreviousPage { get; init; }
    public bool HasNextPage { get; init; }
    public bool IsFirstPage { get; init; }
    public bool IsLastPage { get; init; }
    public int FirstItemOnPage { get; init; }
    public int LastItemOnPage { get; init; }

    public bool IsPaginated => PageCount > 1;
    public int TotalPages => (int)Math.Ceiling((double)TotalItemCount / PageSize);
}
