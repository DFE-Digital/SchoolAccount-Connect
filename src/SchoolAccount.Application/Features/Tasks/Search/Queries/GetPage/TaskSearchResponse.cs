namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskSearchResponse(
    IReadOnlyCollection<TaskListItem> Tasks
);
