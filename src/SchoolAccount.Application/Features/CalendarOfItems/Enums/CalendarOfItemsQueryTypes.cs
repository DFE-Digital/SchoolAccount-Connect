namespace SchoolAccount.Application.Features.CalendarOfItems.Enums;

[Flags]
public enum CalendarOfItemsQueryTypes
{
    None = 0,
    SubTask = 1 << 0,
    Task = 1 << 1,
}
