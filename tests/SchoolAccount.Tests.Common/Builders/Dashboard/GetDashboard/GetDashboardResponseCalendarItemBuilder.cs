using SchoolAccount.Application.Features.Dashboard;

namespace SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard;

public class GetDashboardResponseCalendarItemBuilder
{
    private long _id = 1;
    private string _name = "Calendar item name";
    private string? _description;
    private DateOnly? _sortDate;
    private DateTime? _lastUpdated;
    private DateOnly? _dueDate;
    private bool? _dueDateIsExact;
    private DateOnly? _startDate;
    private bool? _startDateIsExact;
    private string? _dateText;

    public static GetDashboardResponseCalendarItemBuilder AResponseCalendarItem() => new();

    public GetDashboardResponseCalendarItemBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithSortDate(DateOnly sortDate)
    {
        _sortDate = sortDate;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithLastUpdated(DateTime lastUpdated)
    {
        _lastUpdated = lastUpdated;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithDueDate(DateOnly dueDate, bool isExact = true)
    {
        _dueDate = dueDate;
        _dueDateIsExact = isExact;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithStartDate(DateOnly startDate, bool isExact = true)
    {
        _startDate = startDate;
        _startDateIsExact = isExact;
        return this;
    }

    public GetDashboardResponseCalendarItemBuilder WithDateText(string dateText)
    {
        _dateText = dateText;
        return this;
    }

    private GetDashboardResponseCalendarItem Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            Description = _description,
            SortDate = _sortDate,
            LastUpdated = _lastUpdated,
            DueDate = _dueDate,
            DueDateIsExact = _dueDateIsExact,
            StartDate = _startDate,
            StartDateIsExact = _startDateIsExact,
            DateText = _dateText,
        };

    public static GetDashboardResponseCalendarItem[] BuildMany(
        IEnumerable<GetDashboardResponseCalendarItemBuilder> builders
    ) => builders.Select(builder => builder.Build()).ToArray();

    public static implicit operator GetDashboardResponseCalendarItem(GetDashboardResponseCalendarItemBuilder builder) =>
        builder.Build();
}
