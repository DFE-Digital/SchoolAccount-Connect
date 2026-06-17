namespace SchoolAccount.Application.Features.Dashboard;

public sealed record GetDashboardResponse
{
    public required IReadOnlyList<GetDashboardResponseCalendarItem> CalendarOfItems { get; init; }

    public required IReadOnlyList<GetDashboardResponseCategoryItem> Categories { get; init; }
}

public sealed record GetDashboardResponseCalendarItem
{
    public long Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public string? Description { get; init; }

    public DateOnly? SortDate { get; init; }

    public DateTime? LastUpdated { get; init; }

    public DateOnly? DueDate { get; init; }

    public bool? DueDateIsExact { get; init; }

    public DateOnly? StartDate { get; init; }

    public bool? StartDateIsExact { get; init; }

    public string? DateText { get; init; }
}

public sealed record GetDashboardResponseCategoryItem
{
    public long Id { get; init; }

    public string DisplayName { get; init; } = string.Empty;

    public string? Description { get; init; }
}
