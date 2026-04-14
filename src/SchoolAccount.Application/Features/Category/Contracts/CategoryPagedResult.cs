using SchoolAccount.Application.Features.Category.Models;
using X.PagedList;

namespace SchoolAccount.Application.Features.Category.Contracts;

public class CategoryPagedResult(
    IPagedList<CategoryRow> payload)
    : IPagedList
{
    public IReadOnlyCollection<CategoryRow> Payload { get; } = payload;
    public int PageCount { get; } = payload.PageCount;
    public int TotalItemCount { get; } = payload.TotalItemCount;
    public int PageNumber { get; } = payload.PageNumber;
    public int PageSize { get; } = payload.PageSize;
    public bool HasPreviousPage { get; } = payload.HasPreviousPage;
    public bool HasNextPage { get; } = payload.HasNextPage;
    public bool IsFirstPage { get; } = payload.IsFirstPage;
    public bool IsLastPage { get; } = payload.IsLastPage;
    public int FirstItemOnPage { get; } = payload.FirstItemOnPage;
    public int LastItemOnPage { get; } = payload.LastItemOnPage;
}
