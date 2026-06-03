using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Shared.Filtering.Models;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record GetSubTasksByDirectionForTabViewCalendarOfItemsQuery : CalendarOfItemsDirectionalQuery
{
    public GetSubTasksByDirectionForTabViewCalendarOfItemsQuery(
        CalendarOfItemsViewModes viewModes,
        int pageSize = 10,
        int pageNumber = 1,
        Dictionary<string, List<string>>? filters = null,
        CalendarOfItemsSortMode sortMode = CalendarOfItemsSortMode.NotSpecified,
        DateOnly? date = null
    )
        : base(
            viewModes == CalendarOfItemsViewModes.None ? CalendarOfItemsViewModes.Forward : viewModes,
            12,
            date ?? DateOnlyExtensions.Today,
            pageSize <= 0 ? 10 : pageSize,
            pageNumber <= 0 ? 1 : pageNumber,
            sortMode,
            BuildFilter(filters ?? [])
        ) { }

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
