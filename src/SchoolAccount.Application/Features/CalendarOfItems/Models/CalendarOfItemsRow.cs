using SchoolAccount.Application.Features.CalendarOfItems.Enums;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsRow
{
    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }

    public long? Id { get; init; }

    public string? Name { get; init; }

    public string? Description { get; init; }

    public CalendarOfItemsRowStatus? Status { get; init; }

    public DateOnly? SortDate { get; init; }

    public CalendarOfItemsRowType Type { get; init; } = CalendarOfItemsRowType.None;
    public DateTime? LastUpdated { get; set; }
}
