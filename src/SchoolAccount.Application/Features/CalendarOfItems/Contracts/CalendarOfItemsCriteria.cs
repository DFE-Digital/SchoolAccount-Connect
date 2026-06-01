using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Contracts;

public interface IQueryCriteria
{
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public IList<FilterRequest> Filter { get; init; }
    public bool PopulateFilterOptions { get; init; }
    public GenericOrderFunction<QueryRow>? CustomOrderByFunction { get; init; }
}

public class CalendarOfItemsCriteria : IQueryCriteria
{
    public CalendarOfItemsQueryTypes ToQuery { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public DateOnlyRange Range { get; init; }
    public CalendarOfItemsViewModes ViewModes { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public IList<FilterRequest> Filter { get; init; } = [];
    public bool PopulateFilterOptions { get; init; } = true;
    public GenericOrderFunction<QueryRow>? CustomOrderByFunction { get; init; }
}
