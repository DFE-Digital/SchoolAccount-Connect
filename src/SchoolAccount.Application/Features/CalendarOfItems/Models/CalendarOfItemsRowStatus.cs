using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class CalendarOfItemsRowStatus : ExtensionNode<int>
{
    public string? Theme { get; init; }

    public long? EntityId { get; init; }
}
