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
}
