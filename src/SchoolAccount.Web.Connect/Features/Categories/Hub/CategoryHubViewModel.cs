using SchoolAccount.Application.Features.Categories.GetCategoryHub;
using SchoolAccount.Web.Connect.Models.Shared;
using X.PagedList.Extensions;

namespace SchoolAccount.Web.Connect.Features.Categories.Hub;

public sealed record CategoryHubViewModel(GetCategoryHubResponse CategoryHubResponse, PaginationViewModel Pagination)
    : ITaskListViewModel
{
    public GetCategoryHubResponseCategory Category => CategoryHubResponse.Category;

    public IReadOnlyCollection<ListItemViewModel> Tasks =>
        CategoryHubResponse.Tasks.Select(t => new ListItemViewModel(
            t.Name,
            string.Format(Thread.CurrentThread.CurrentCulture, RouteConstants.Task.Index, t.Id),
            description: t.Requirement.ToString()
        ));

    public bool NoTasksFound => Tasks.Count == 0;

    public string NoTasksFoundMessage => $"No tasks found for the category {Category.Name}.";

    public bool IsAcademyTrustHandbookCategory => Category.Id == 1;
}
