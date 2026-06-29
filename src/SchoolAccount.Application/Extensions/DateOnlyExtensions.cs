namespace SchoolAccount.Application.Extensions;

public static class DateOnlyExtensions
{
    public static DateOnly StartOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, 1);
    }

    public static DateOnly EndOfMonth(this DateOnly date)
    {
        return new DateOnly(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
    }

    public static DateOnly Today => new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

    public static string ToGdsDateString(this DateOnly? date)
    {
        return date.HasValue ? date.Value.ToGdsDateString() : string.Empty;
    }
    public static string ToGdsDateString(this DateOnly date)
    {
        return date.ToString(FormattingConstants.DateMonthYearFormat, null) ;
    }

    public static string ToGdsMonthString(this DateOnly? date)
    {
        return date.HasValue ? date.Value.ToGdsMonthString() : string.Empty;
    }

    public static string ToGdsMonthString(this DateOnly date)
    {
        return date.ToString(FormattingConstants.MonthYearFormat, null);
    }
}
