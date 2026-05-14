using System.Globalization;
using SchoolAccount.Domain.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.Common.Labels;

public class SubTaskAvailabilityLabel
{
    private const string AvailableNow = "Available Now.";
    private static readonly TimeZoneInfo UkTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

    private readonly IDateTimeProvider _dateTimeProvider;

    public SubTaskAvailabilityLabel(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public string Generate(WorkflowState workflowState, DateOnly? startDate, bool? startDateIsExact, DateOnly? dueDate)
    {
        var (ukNow, todayInUk) = ResolveUkTimeContext();

        return workflowState switch
        {
            WorkflowState.Published => GetPublishedLabel(startDate, startDateIsExact, dueDate, ukNow, todayInUk),
            WorkflowState.Expired => GetExpiredLabel(startDate, startDateIsExact),
            _ => string.Empty,
        };
    }

    private static string GetPublishedLabel(
        DateOnly? start,
        bool? isExact,
        DateOnly? due,
        DateTime ukNow,
        DateOnly todayInUk
    )
    {
        return (start, isExact) switch
        {
            (not null, true) => HasReachedExactStartDate(start.Value, todayInUk)
                ? AvailableNow
                : FormatLabel(start.Value, isExact: true),

            (not null, false) => HasEnteredFollowingMonth(start.Value, ukNow)
                ? AvailableNow
                : FormatLabel(start.Value, isExact: false),

            (null, _) when due is not null => AvailableNow,

            _ => string.Empty,
        };
    }

    private (DateTime ukNow, DateOnly todayInUk) ResolveUkTimeContext()
    {
        var ukNow = TimeZoneInfo.ConvertTimeFromUtc(_dateTimeProvider.UtcNow, UkTimeZone);
        return (ukNow, DateOnly.FromDateTime(ukNow));
    }

    private static bool HasReachedExactStartDate(DateOnly startDate, DateOnly todayInUk)
    {
        return startDate <= todayInUk;
    }

    private static bool HasEnteredFollowingMonth(DateOnly date, DateTime ukNow)
    {
        var firstOfNextMonth = new DateTime(date.Year, date.Month, 1, 0, 0, 0).AddMonths(1);
        return ukNow >= firstOfNextMonth;
    }

    private static string GetExpiredLabel(DateOnly? start, bool? isExact)
    {
        return start is not null && isExact is not null ? FormatLabel(start.Value, isExact.Value) : string.Empty;
    }

    private static string FormatLabel(DateOnly date, bool isExact)
    {
        var format = isExact ? "d MMM yyyy" : "MMM yyyy";
        return $"Available {date.ToString(format, CultureInfo.CurrentCulture)}.";
    }
}
