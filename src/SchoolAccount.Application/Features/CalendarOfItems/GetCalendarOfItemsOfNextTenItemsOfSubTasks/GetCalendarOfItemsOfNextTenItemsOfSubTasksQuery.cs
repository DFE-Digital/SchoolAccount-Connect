using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;
using SchoolAccount.Application.Features.CalendarOfItems.Common.Models;
using SchoolAccount.Application.Features.Shared.Filtering.Filters;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Application.Features.Shared.Query.Contracts;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.GetCalendarOfItemsOfNextTenItemsOfSubTasks;

public record GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery : IQuery<QueryPagedResult<CalendarOfItemsRow>>
{
    public GetCalendarOfItemsOfNextTenItemsOfSubTasksQuery(DateOnly date)
    {
        QueryRange = new DateOnlyRange(date.StartOfMonth(), date.AddMonths(12).EndOfMonth());
        PageSize = 10;
        PageNumber = 1;
        SortMode = CalendarOfItemsSortMode.NotSpecified;
        NoResultMessage = $"No tasks coming up over the next 12 months";
        Filter = BuildFilter();
    }

    public DateOnlyRange QueryRange { get; init; }
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
    public CalendarOfItemsSortMode SortMode { get; init; }
    public string NoResultMessage { get; init; }
    public IList<FilterRequest>? Filter { get; init; }

    private static CalendarOfItemsFilter BuildFilter()
    {
        return new CalendarOfItemsFilter([
            new FilterRequest
            {
                Field = SubTaskFilterableRegistrar.Keys.State,
                Operator = ComparisonType.Equals,
                Value = WorkflowState.Published,
            },
        ]);
    }
}
