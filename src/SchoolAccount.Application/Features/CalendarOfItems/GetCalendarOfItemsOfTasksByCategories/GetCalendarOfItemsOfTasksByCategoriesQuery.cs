using System.Collections.ObjectModel;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfTasksByCategories;

public record GetCalendarOfItemsOfTasksByCategoriesQuery : IQuery<GenericQueryPagedResult<CalendarOfItemsRow>>
{
    public GetCalendarOfItemsOfTasksByCategoriesQuery(
        Collection<int> categoryIds,
        int pageSize = 10,
        int pageNumber = 1,
        DateOnly? date = null
    )
    {
        QueryRange = BuildDateRange(date);
        PageSize = pageSize <= 0 ? 10 : pageSize;
        PageNumber = pageNumber <= 0 ? 1 : pageNumber;
        SortMode = CalendarOfItemsSortMode.NotSpecified;
        NoResultMessage = "No results found";
        Filter = BuildFilter(categoryIds);
    }

    public DateOnlyRange QueryRange { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public string NoResultMessage { get; init; }
    public IList<FilterRequest>? Filter { get; init; }

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
                    Field = TaskFilterableRegistrar.Keys.Categories,
                    Operator = ComparisonType.In,
                    Value = categoryIds,
                }
            );
        }

        return new CalendarOfItemsFilter(options);
    }
}
