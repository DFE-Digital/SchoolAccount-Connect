using SchoolAccount.Application.Features.Calendars.CalendarList.Enums;

namespace SchoolAccount.Application.Features.Calendars.CalendarList.Models;

public class CalendarOfItemsRow
{
    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }

    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public CalendarOfItemsRowStatus? Status { get; init; }

    public DateOnly? SortDate { get; init; }

    public CalendarOfItemsRowType Type { get; init; } = CalendarOfItemsRowType.None;

    public DateTime? LastUpdated { get; init; }

    public IEnumerable<CalendarOfItemsExtensionNode> Types { get; init; } = [];

    public IEnumerable<CalendarOfItemsExtensionNode> Tags { get; init; } = [];
}
