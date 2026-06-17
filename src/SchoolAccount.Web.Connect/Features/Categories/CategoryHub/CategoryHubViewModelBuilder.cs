using SchoolAccount.Application.Common;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Web.Connect.Features.Shared.ListItem;
using X.PagedList;

namespace SchoolAccount.Web.Connect.Features.Categories.CategoryHub;

public static class CategoryHubViewModelBuilder
{
    public static CategoryHubViewModel Build(GetCategoryHubResponse response)
    {
        return new CategoryHubViewModel
        {
            CategoryId = response.Id,
            Name = response.DisplayName,
            HubViewDescription = response.HubViewDescription,
            Tasks = new PaginatedListViewModel(
                MapTasksToPagedList(response.Tasks),
                $"No tasks found for the category {response.DisplayName}."
            ),
        };
    }

    private static IPagedList<ListItemViewModel> MapTasksToPagedList(
        PagedResult<GetCategoryHubResponseTasks> pagedTasks
    )
    {
        var taskViewModels = pagedTasks.Items.Select(task => new ListItemViewModel(
            task.Name,
            string.Format(Thread.CurrentThread.CurrentCulture, RouteConstants.Task.Index, task.Id),
            description: task.Description ?? (task.Requirement != null ? $"{task.Requirement} task" : string.Empty)
        ));

        return taskViewModels.ToStaticPagedList(pagedTasks.PageNumber, pagedTasks.PageSize, pagedTasks.TotalCount);
    }
}
