namespace SchoolAccount.Application.Features.Category.Enums;

[Flags]
public enum CategoryListViewModes
{
    None = 0,
    Forward = 1 << 0,
    Backward = 1 << 1,
    Custom = 1 << 2,
    Standalone = 1 << 3,
    Dashboard = 1 << 4,
}
