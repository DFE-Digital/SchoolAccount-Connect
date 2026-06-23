using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Dashboard;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Dashboard.Handlers;

public class TestGetDashboardQueryHandler : IQueryHandler<GetDashboardQuery, GetDashboardResponse>
{
    private readonly List<GetDashboardResponseCalendarItem> _calendarItems = [];
    private readonly List<GetDashboardResponseCategoryItem> _categories = [];

    public async Task<Result<GetDashboardResponse>> Handle(GetDashboardQuery query, CancellationToken cancellationToken)
    {
        var response = new GetDashboardResponse { CalendarOfItems = _calendarItems, Categories = _categories };

        return await Task.FromResult(Result.Success(response));
    }

    public TestGetDashboardQueryHandler AddCalendarItem(GetDashboardResponseCalendarItem item)
    {
        _calendarItems.Add(item);
        return this;
    }

    public TestGetDashboardQueryHandler AddCategory(GetDashboardResponseCategoryItem category)
    {
        _categories.Add(category);
        return this;
    }

    public void Clear()
    {
        _calendarItems.Clear();
        _categories.Clear();
    }
}
