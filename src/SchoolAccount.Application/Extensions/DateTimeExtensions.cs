namespace SchoolAccount.Application.Extensions;

public static class DateTimeExtensions
{
    public static DateTime StartOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, 1);
    }

    public static DateTime EndOfMonth(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateOnly ToDateOnly(this DateTime date)
    {
        return new DateOnly(date.Year, date.Month, date.Day);
    }

    public static string ToGdsDateString(this DateTime date, bool includeTime = false)
    {
        var formatted = date.ToString(FormattingConstants.DateMonthYearFormat, null);

        if (!includeTime)
        {
            return formatted;
        }

        var time = date.ToString(FormattingConstants.TimeFormat, null).ToLower(null);
        formatted += $" at {time}";

        return formatted;
    }

    public static string ToGdsDateString(this DateTime? date, bool includeTime = false)
    {
        return date is null ? string.Empty : ((DateTime)date).ToGdsDateString(includeTime);
    }

    public static string ToGdsMonthString(this DateTime? date)
    {
        return date.HasValue ? date.Value.ToString(FormattingConstants.MonthYearFormat, null) : string.Empty;
    }

    public static string ToGdsMonthString(this DateTime date)
    {
        return date.ToString(FormattingConstants.MonthYearFormat, null);
    }
}
