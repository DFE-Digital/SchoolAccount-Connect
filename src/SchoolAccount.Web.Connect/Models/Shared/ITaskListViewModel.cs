namespace SchoolAccount.Web.Connect.Models.Shared;

public interface ITaskListViewModel
{
    IReadOnlyCollection<ListItemViewModel> Tasks { get; }
    PaginationViewModel Pagination { get; }
    bool NoTasksFound { get; }
    string NoTasksFoundMessage { get; }
}
