using System.Collections.ObjectModel;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;
using X.PagedList;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public class QueryPagedResult<TRow>(
    GenericQueryCriteria<TRow> criteria,
    IPagedList<TRow> payload,
    Collection<Filterable> filter
) : IPagedList
where TRow: IQueryRow
{
    public DateTime GeneratedDate { get; } = DateTime.UtcNow;
    
    public DateOnlyRange Range { get; } = criteria.Range;
    public Collection<Filterable> Filter { get; } = filter;
    public IReadOnlyCollection<TRow> Payload { get; } = payload;
    
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
