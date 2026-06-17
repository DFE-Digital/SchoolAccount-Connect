using SchoolAccount.Web.Connect.Features.Shared.ListItem;

namespace SchoolAccount.Web.Connect.Features.Tasks.GetAll;

public sealed record GetAllTasksViewModel
{
    public required PaginatedListViewModel Tasks { get; init; }

    public string Heading => "All tasks";

    public string Description => "Explore all tasks and support.";

    public string HubViewDescription => "See all your tasks, returns and policies from DfE.";
}
