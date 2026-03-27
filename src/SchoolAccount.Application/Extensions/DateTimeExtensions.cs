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
        var formatted = date.ToString("d MMMM yyyy", null);

        if (!includeTime)
        {
            return formatted;
        }

        var time = date.ToString("h:mmtt", null).ToLower(null);
        formatted += $" at {time}";

        return formatted;
    }
}
