namespace SchoolAccount.Domain.Dtos;

public sealed record TaskWithSubTasksDto(
    IReadOnlyCollection<TaskListItemDto> Tasks,
    IReadOnlyCollection<SubTaskListItemDto> SubTasks
);
