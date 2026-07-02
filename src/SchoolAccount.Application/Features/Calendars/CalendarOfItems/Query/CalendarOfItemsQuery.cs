using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Contracts;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;

namespace SchoolAccount.Application.Features.Calendars.CalendarOfItems.Query;

public sealed record CalendarOfItemsQuery : IQuery<CalendarOfItemsResponse>
{
    public CalendarOfItemsQuery(
        CalendarOfItemsViewModes viewModes,
        int pageSize = 10,
        int pageNumber = 1,
        Dictionary<string, List<string>>? filters = null,
        CalendarOfItemsSortMode sortMode = CalendarOfItemsSortMode.NotSpecified,
        DateOnly? date = null
    )
    {
        ViewModes = viewModes == CalendarOfItemsViewModes.None ? CalendarOfItemsViewModes.Forward : viewModes;
        QueryFromDate = date ?? DateOnlyExtensions.Today;
        PageSize = pageSize <= 0 ? 10 : pageSize;
        PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        SortMode = sortMode;
        Filter = BuildFilter(filters ?? []);
    }

    public CalendarOfItemsQueryTypes ToQuery { get; } = CalendarOfItemsQueryTypes.SubTask;

    public CalendarOfItemsViewModes ViewModes { get; }

    public int ViewPeriodInMonths { get; } = 12;

    public DateOnly QueryFromDate { get; }

    public int PageSize { get; }

    public int PageNumber { get; }

    public CalendarOfItemsSortMode SortMode { get; }

    public CalendarOfItemsFilter Filter { get; }

    public CalendarOfItemsOrderFunction? CustomOrderBy { get; }

    private static CalendarOfItemsFilter BuildFilter(Dictionary<string, List<string>> filters)
    {
        return new CalendarOfItemsFilter(
            filters.Select(filter => new FilterRequest
            {
                Field = filter.Key,
                Operator = filter.Key switch
                {
                    "name" => ComparisonType.Contains,
                    _ => ComparisonType.In,
                },
                Value = filter.Key switch
                {
                    "name" => filter.Value,
                    _ => filter.Value.GetType() == typeof(string)
                        ? filter.Value.ToString()?.Split(',').ToList()
                        : filter.Value,
                },
            })
        );
    }
}
