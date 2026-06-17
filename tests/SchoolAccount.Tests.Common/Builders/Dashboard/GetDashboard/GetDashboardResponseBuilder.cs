using SchoolAccount.Application.Features.Dashboard;

namespace SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard;

public class GetDashboardResponseBuilder
{
    private List<GetDashboardResponseCalendarItem> _calendarOfItems = [];
    private List<GetDashboardResponseCategoryItem> _categories = [];

    public static GetDashboardResponseBuilder AResponse() => new();

    public GetDashboardResponseBuilder WithCalendarOfItems(params GetDashboardResponseCalendarItem[] calendarOfItems)
    {
        _calendarOfItems = [.. calendarOfItems];
        return this;
    }

    public GetDashboardResponseBuilder WithCategories(params GetDashboardResponseCategoryItem[] categories)
    {
        _categories = [.. categories];
        return this;
    }

    private GetDashboardResponse Build() => new() { CalendarOfItems = _calendarOfItems, Categories = _categories };

    public static implicit operator GetDashboardResponse(GetDashboardResponseBuilder builder) => builder.Build();
}
