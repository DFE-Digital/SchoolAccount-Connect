using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public interface IQueryCriteria<TRow>
where TRow: IQueryRow
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public IList<FilterRequest> Filter { get; init; }
    public bool PopulateFilterOptions { get; init; }
    public GenericOrderFunction<TRow>? CustomOrderByFunction { get; init; }
}

public class GenericQueryCriteria<TRow> : IQueryCriteria<TRow>
where TRow: IQueryRow
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public IList<FilterRequest> Filter { get; init; } = [];
    public bool PopulateFilterOptions { get; init; } = true;
    public GenericOrderFunction<TRow>? CustomOrderByFunction { get; init; }
}

public class CalendarOfItemsQueryCriteria : GenericQueryCriteria<CalendarOfItemsRow>
{
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
}
