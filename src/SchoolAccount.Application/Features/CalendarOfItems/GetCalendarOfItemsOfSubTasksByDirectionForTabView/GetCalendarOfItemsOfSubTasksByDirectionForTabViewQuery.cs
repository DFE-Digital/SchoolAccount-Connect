using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfSubTasksByDirectionForTabView;

public record GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery
    : IQuery<GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public GetCalendarOfItemsOfSubTasksByDirectionForTabViewQuery(
        CalendarOfItemsViewModes viewModes,
        int pageSize = 10,
        int pageNumber = 1,
        Dictionary<string, List<string>>? filters = null,
        CalendarOfItemsSortMode sortMode = CalendarOfItemsSortMode.NotSpecified,
        DateOnly? date = null
    )
    {
        ViewModes = viewModes == CalendarOfItemsViewModes.None ? CalendarOfItemsViewModes.Forward : viewModes;
        ViewPeriodInMonths = 12;
        QueryFromDate = date ?? DateOnlyExtensions.Today;
        PageSize = pageSize <= 0 ? 10 : pageSize;
        PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        SortMode = sortMode;
        Filter = BuildFilter(filters);
    }

    public CalendarOfItemsViewModes ViewModes { get; init; }
    public int ViewPeriodInMonths { get; init; }
    public DateOnly QueryFromDate { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public IList<FilterRequest>? Filter { get; init; }

    private static CalendarOfItemsFilter BuildFilter(Dictionary<string, List<string>>? filters)
    {
        filters ??= [];

        return new CalendarOfItemsFilter(
            filters.Select(filter => new FilterRequest
            {
                Field = filter.Key,
                Operator = filter.Key switch
                {
                    SubTaskFilterableRegistrar.Keys.Name => ComparisonType.Contains,
                    _ => ComparisonType.In,
                },
                Value = filter.Key switch
                {
                    SubTaskFilterableRegistrar.Keys.Name => filter.Value,
                    _ => filter.Value.GetType() == typeof(string)
                        ? filter.Value.ToString()?.Split(',').ToList()
                        : filter.Value,
                },
            })
        );
    }
}
