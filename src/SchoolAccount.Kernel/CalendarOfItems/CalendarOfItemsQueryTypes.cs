namespace SchoolAccount.Kernel.CalendarOfItems;

[Flags]
public enum CalendarOfItemsQueryTypes
{
    None = 0,
    SubTask = 1 << 0,
}
