namespace SchoolAccount.Domain.Dtos;

public sealed record TaskListItemDto(long Id, string ReferenceNo, string Name, string UpdatedBy, DateTime DateUpdated);

public sealed record TaskListItemWithSubTaskList(
    TaskListItemDto Task,
    IReadOnlyCollection<SubTaskListItemDto> SubTasks
);
