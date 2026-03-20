using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Kernel;
using X.PagedList;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public class CalendarOfItemsPagedResult(CalendarOfItemsCriteria criteria, IPagedList<CalendarOfItemsRow> payload)
    : IPagedList
{
    public CalendarOfItemsViewMode ViewMode { get; } = criteria.ViewMode;
    public DateTime GeneratedDate { get; } = DateTime.UtcNow;
    public DateOnlyRange QueryRange { get; } = criteria.Range;
    public IReadOnlyCollection<CalendarOfItemsRow> Payload { get; } = payload;
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
