namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskWithSubTasksDto(
    IReadOnlyCollection<TaskListItemDto> Tasks
);
