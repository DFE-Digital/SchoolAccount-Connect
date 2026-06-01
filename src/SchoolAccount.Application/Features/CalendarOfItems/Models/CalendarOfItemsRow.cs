using SchoolAccount.Application.Features.CalendarOfItems.Enums;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.CalendarOfItems.Models;

public class QueryRow : Node<long>
{
    public string? Description { get; init; }
    public DateOnly? SortDate { get; init; }
    public Node<int> Type { get; init; } = 0;
    public DateTime? LastUpdated { get; init; }
    public ExtensionNode<int>? Status { get; init; }
    public IEnumerable<ExtensionNode<long>> Types { get; init; } = [];
    public IEnumerable<ExtensionNode<long>> Tags { get; init; } = [];
}

public class CalendarOfItemsRow : QueryRow
{
    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }
}
