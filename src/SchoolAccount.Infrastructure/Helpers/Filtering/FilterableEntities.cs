namespace SchoolAccount.Infrastructure.Helpers.Filtering;

[Flags]
public enum FilterableEntities
{
    None = 0,
    SubTask = 1 << 0,
    Task = 1 << 1,
}