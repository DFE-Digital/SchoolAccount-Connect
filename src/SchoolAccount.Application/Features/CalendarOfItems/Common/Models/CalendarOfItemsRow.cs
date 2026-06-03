using SchoolAccount.Application.Features.Shared.Query.Models;

namespace SchoolAccount.Application.Features.CalendarOfItems.Common.Models;

public class CalendarOfItemsRow : QueryRow
{
    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }
}
