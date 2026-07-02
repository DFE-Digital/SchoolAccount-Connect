using System.Globalization;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Features.Shared.List;
using SchoolAccount.Web.Connect.Features.Shared.Pagination;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Features.Tasks.GetAll;

public static class GetAllTasksViewModelBuilder
{
    public static GetAllTasksViewModel Build(GetAllTasksResponse response) =>
        new() { Tasks = new PaginatedListViewModel(MapTasksToPagedList(response.Tasks), "No tasks found.") };

    private static IPagedList<ListItemViewModel> MapTasksToPagedList(PagedResult<GetAllTasksResponseTask> pagedTasks)
    {
        return pagedTasks
            .Items.Select(ToListItem)
            .ToList()
            .ToStaticPagedList(pagedTasks.PageNumber, pagedTasks.PageSize, pagedTasks.TotalCount);
    }

    private static ListItemViewModel ToListItem(GetAllTasksResponseTask task) =>
        new(
            task.Name,
            string.Format(CultureInfo.InvariantCulture, RouteConstants.Task.Index, task.Id),
            description: task.Description ?? (task.Requirement != null ? $"{task.Requirement.ToString()} task" : null)
        );
}
