using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.Search;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Shared.ListItem;
using X.PagedList;
using static SchoolAccount.Web.Connect.RouteConstants;

namespace SchoolAccount.Web.Connect.Features.Tasks.Search;

public static class SearchTasksViewModelBuilder
{
    public static SearchTasksViewModel Build(SearchTasksResponse searchTasksResponse, string searchTerm)
    {
        return new SearchTasksViewModel
        {
            SearchTerm = searchTerm,
            Tasks = new PaginatedListViewModel(
                MapSearchResultsToPagedList(searchTasksResponse.Tasks),
                "No tasks found."
            ),
            Description = BuildDescription(searchTerm),
            SubHeading = BuildSubHeading(searchTasksResponse.Tasks),
        };
    }

    private static IPagedList<ListItemViewModel> MapSearchResultsToPagedList(
        PagedResult<SearchTasksResponseTask> pagedTasks
    )
    {
        return pagedTasks
            .Items.Select(ToListItem)
            .ToList()
            .ToStaticPagedList(pagedTasks.PageNumber, pagedTasks.PageSize, pagedTasks.TotalCount);
    }

    private static string BuildDescription(string searchTerm)
    {
        return string.IsNullOrWhiteSpace(searchTerm)
            ? "Showing matching tasks."
            : $"Showing results for “{searchTerm}”.";
    }

    private static string BuildSubHeading(PagedResult<SearchTasksResponseTask> tasks)
    {
        if (tasks.TotalCount == 0)
        {
            return "No tasks found.";
        }

        var taskCount = tasks.TotalCount;
        var taskWord = taskCount == 1 ? "task" : "tasks";

        return $"{taskCount} {taskWord} found.";
    }

    private static ListItemViewModel ToListItem(SearchTasksResponseTask task) =>
        new(task.Name, Url(RouteConstants.Task.Index, task.Id), description: task.Description);
}
