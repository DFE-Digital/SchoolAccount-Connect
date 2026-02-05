namespace SchoolAccount.Application.Features.Tasks.Search.Queries;

public sealed record TaskSearchResultsVm(
    string Term,
    IReadOnlyList<TaskSearchResultVm> Results);

public sealed record TaskSearchResultVm(
    Guid TaskId,
    string TaskTitle,
    IReadOnlyList<SubtaskMatchVm> MatchingSubtasks);

public sealed record SubtaskMatchVm(
    Guid SubtaskId,
    string Title,
    string Status,
    string? ScheduleSummary);