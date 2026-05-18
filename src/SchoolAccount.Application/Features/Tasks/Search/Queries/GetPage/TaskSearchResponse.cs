using X.PagedList;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed class TaskSearchResponse(IPagedList<TaskListItem> tasks) : IPagedList
{
    public IReadOnlyCollection<TaskListItem> Tasks { get; } = tasks;

    public int PageCount { get; } = tasks.PageCount;
    public int TotalItemCount { get; } = tasks.TotalItemCount;
    public int PageNumber { get; } = tasks.PageNumber;
    public int PageSize { get; } = tasks.PageSize;
    public bool HasPreviousPage { get; } = tasks.HasPreviousPage;
    public bool HasNextPage { get; } = tasks.HasNextPage;
    public bool IsFirstPage { get; } = tasks.IsFirstPage;
    public bool IsLastPage { get; } = tasks.IsLastPage;
    public int FirstItemOnPage { get; } = tasks.FirstItemOnPage;
    public int LastItemOnPage { get; } = tasks.LastItemOnPage;
}
