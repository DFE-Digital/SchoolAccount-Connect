using SchoolAccount.Application.Features.Calendars.CalendarOfItems.Enums;
using SchoolAccount.InfrastructureTests.Builders;
using SchoolAccount.Kernel;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class CalendarOfItemsRowExtensions
{
    public static CalendarOfItemsRowBuilder Create(
        long id,
        string name,
        DateOnly? sortDate,
        CalendarOfItemsRowType? type = null
    )
    {
        return new CalendarOfItemsRowBuilder(id, name, sortDate, type ?? CalendarOfItemsRowType.SubTask);
    }

    public static CalendarOfItemsRowBuilder Create(DateOnly sortDate)
    {
        return new CalendarOfItemsRowBuilder(0, string.Empty, sortDate, CalendarOfItemsRowType.SubTask);
    }

    public static CalendarOfItemsRowBuilder Create(string name, DateOnly sortDate)
    {
        return new CalendarOfItemsRowBuilder(0, name, sortDate, CalendarOfItemsRowType.SubTask);
    }

    public static CalendarOfItemsRowBuilder Create(int id, DateOnly sortDate)
    {
        return new CalendarOfItemsRowBuilder(id, string.Empty, sortDate, CalendarOfItemsRowType.SubTask);
    }

    public static CalendarOfItemsRowBuilder Create()
    {
        return new CalendarOfItemsRowBuilder(0, string.Empty, null, CalendarOfItemsRowType.SubTask);
    }

    public static CalendarOfItemsRowCollectionBuilder Collection(DateOnlyRange range)
    {
        return new CalendarOfItemsRowCollectionBuilder(range);
    }
}
