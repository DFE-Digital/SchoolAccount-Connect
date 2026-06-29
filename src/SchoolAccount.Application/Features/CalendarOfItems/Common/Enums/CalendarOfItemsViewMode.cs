namespace SchoolAccount.Application.Features.CalendarOfItems.Common.Enums;

[Flags]
public enum CalendarOfItemsViewModes
{
    None = 0,
    Forward = 1 << 0,
    Backward = 1 << 1,
    Custom = 1 << 2,
    Standalone = 1 << 3,
    Hub = 1 << 4,
    List = 1 << 5,
    Kanban = 1 << 6,
}
