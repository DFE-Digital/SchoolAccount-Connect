
using SchoolAccount.Application.Features.Shared;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskListItem(
    long Id,
    string ReferenceNo,
    string Name,
    string UpdatedBy,
    DateTime DateUpdated);

public sealed record SubTaskListItem(
    long Id,
    string Name,
    string UpdatedBy,
    DateTime DateUpdated);

public sealed record TaskWithSubTasks(IReadOnlyCollection<TaskListItem> Tasks,
    IReadOnlyCollection<SubTaskListItem> SubTasks);