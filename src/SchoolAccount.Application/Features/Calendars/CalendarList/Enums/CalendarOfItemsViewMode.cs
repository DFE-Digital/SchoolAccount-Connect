namespace SchoolAccount.Application.Features.Calendars.CalendarList.Enums;

[Flags]
public enum CalendarOfItemsViewModes
{
    None = 0,
    Forward = 1 << 0,
    Backward = 1 << 1,
    Custom = 1 << 2,
    Standalone = 1 << 3,
    Hub = 1 << 4,
}
