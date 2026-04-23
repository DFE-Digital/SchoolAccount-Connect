using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Domain.ViewModels;

namespace SchoolAccount.Application.Features.TaskDetails;

public sealed record TaskDetailQuery(long TaskId, TaskDetailViewMode TabIndex = TaskDetailViewMode.UpcomingTasks)
    : IQuery<TaskDetailsViewModel>;
