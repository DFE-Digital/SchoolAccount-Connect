using SchoolAccount.Application.Features.Tasks.GetAll;
using SchoolAccount.Web.Connect.Models.Shared;

namespace SchoolAccount.Web.Connect.Features.Tasks.GetAll;

public sealed record GetAllTasksViewModel(GetAllTasksResponse GetAllTasksResponse)
{
    public IEnumerable<ListItemViewModel> Tasks =>
        GetAllTasksResponse.Tasks.Select(t => new ListItemViewModel(
            t.Name,
            string.Format(Thread.CurrentThread.CurrentCulture, RouteConstants.Task.Index, t.Id),
            description: t.Requirement.ToString()
        ));

    public string Heading => "All tasks";

    public string Description => "Explore all tasks and support.";

    public string HubViewDescription => "See all your tasks, returns and policies from DfE.";

    public bool NoTasksFound => GetAllTasksResponse.Tasks.Count == 0;

    public string NoTasksFoundMessage => "No tasks found.";

    public string NavBarList => "#ABCDEFGHIJKLMNOPQRSTUVWXYZ";
}
