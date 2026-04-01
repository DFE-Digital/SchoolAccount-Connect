using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Helpers;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.TaskDetails.ViewModels;

public sealed class TaskDetailsHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
    : IQueryHandler<TaskDetailQuery, TaskDetailsViewModel>
{
    public async Task<Result<TaskDetailsViewModel>> Handle(TaskDetailQuery query, CancellationToken cancellationToken)
    {
        var taskEntity = applicationDbContext.Tasks.AsNoTracking().FirstOrDefault(t => t.Id == query.TaskId);

        if (taskEntity == null)
        {
            return new TaskDetailsViewModel(dateTimeProvider);
        }

        var subTaskEntities = applicationDbContext
            .SubTasks.AsNoTracking()
            .Where(st => st.IsDeleted != true)
            .Where(st => st.TaskId == query.TaskId)
            .ToList();

        var subTaskDtos = new List<SubTaskListItemDto>();

        foreach (var subTaskEntity in subTaskEntities)
        {
            subTaskDtos.Add(SubTaskListItemHelper.ToListItem(subTaskEntity));
        }

        var taskDto = new TaskListItemDto(
            query.TaskId,
            taskEntity.ReferenceNo ?? string.Empty,
            taskEntity.Name ?? string.Empty,
            taskEntity.UpdatedBy,
            taskEntity.DateUpdated
        );

        var tasksWithSubtasks = new TaskListItemWithSubTaskList(taskDto, subTaskDtos);

        return Result.Success(new TaskDetailsViewModel(tasksWithSubtasks, query.TabIndex, dateTimeProvider));
    }
}
