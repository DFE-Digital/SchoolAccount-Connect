using System.Collections.ObjectModel;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.CalendarOfItems.Query.Operational;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Query;

public record GetTasksByCategoriesCalendarOfItemsQuery : CalendarOfItemsCustomQuery
{
    public GetTasksByCategoriesCalendarOfItemsQuery(
        Collection<int> categoryIds,
        int pageSize = 10,
        int pageNumber = 1,
        DateOnly? date = null
    )
        : base(
            BuildDateRange(date),
            pageSize <= 0 ? 10 : pageSize,
            pageNumber <= 0 ? 1 : pageNumber,
            CalendarOfItemsSortMode.NotSpecified,
            "No results found",
            BuildFilter(categoryIds),
            x => x.OrderBy(o => o.Name)
        ) { }

    private static DateOnlyRange BuildDateRange(DateOnly? date)
    {
        date ??= DateOnlyExtensions.Today;
        return new DateOnlyRange(date.Value.AddYears(-1).StartOfMonth(), date.Value.AddYears(1).EndOfMonth());
    }

    private static CalendarOfItemsFilter BuildFilter(Collection<int> categoryIds)
    {
        var options = new Collection<FilterRequest>();

        if (categoryIds.Count > 0)
        {
            options.Add(
                new FilterRequest
                {
                    Field = "category",
                    Operator = ComparisonType.In,
                    Value = categoryIds,
                }
            );
        }

        return new CalendarOfItemsFilter(options);
    }
}
