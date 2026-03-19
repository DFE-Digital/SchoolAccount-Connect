using SchoolAccount.Kernel.CalendarOfItems;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsRowStatus
{
    public string DisplayValue { get; init; } = null!;

    public string? Theme { get; init; }

    public CalendarOfItemsRowType? Type { get; init; }

    public long? EntityId { get; init; }
}
