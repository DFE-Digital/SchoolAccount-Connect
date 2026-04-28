using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class CalendarOfItemsRowStatusExtensions
{
    public static CalendarOfItemsRowStatusBuilder Create(string displayValue)
    {
        return new CalendarOfItemsRowStatusBuilder(displayValue);
    }
}