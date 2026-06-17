using SchoolAccount.Application.Common;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Dashboard;

public sealed class GetDashboardResponseEnricher(IDateTimeProvider dateTimeProvider)
{
    private readonly AvailabilityLabel _availabilityLabel = new(dateTimeProvider);

    public GetDashboardResponse Enrich(GetDashboardResponse response)
    {
        return response with { CalendarOfItems = response.CalendarOfItems.Select(EnrichCalendarItem).ToList() };
    }

    private GetDashboardResponseCalendarItem EnrichCalendarItem(GetDashboardResponseCalendarItem item)
    {
        return item with { DateText = BuildDateText(item) };
    }

    private string? BuildDateText(GetDashboardResponseCalendarItem item)
    {
        var dueLabel = DueDateLabel.Generate(item.DueDate, item.DueDateIsExact);
        if (!string.IsNullOrEmpty(dueLabel))
            return dueLabel;

        var availLabel = _availabilityLabel.Generate(
            WorkflowState.Published,
            item.StartDate,
            item.StartDateIsExact,
            item.DueDate
        );

        return string.IsNullOrEmpty(availLabel) ? null : availLabel;
    }
}
