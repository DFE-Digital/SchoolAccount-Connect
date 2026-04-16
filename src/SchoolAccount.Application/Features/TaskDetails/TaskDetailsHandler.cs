using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.TaskDetails.ViewModels;
using SchoolAccount.Domain.Dtos;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.ViewModels;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.TaskDetails;

public sealed class TaskDetailsHandler(IApplicationDbContext applicationDbContext, IDateTimeProvider dateTimeProvider)
    : IQueryHandler<TaskDetailQuery, TaskDetailsViewModel>
{
    public async Task<Result<TaskDetailsViewModel>> Handle(TaskDetailQuery query, CancellationToken cancellationToken)
    {
        var taskEntity = await applicationDbContext
            .Tasks.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == query.TaskId, cancellationToken);

        if (taskEntity == null)
        {
            return new TaskDetailsViewModel(dateTimeProvider);
        }

        var subTaskEntities = await applicationDbContext
            .SubTasks.AsNoTracking()
            .Where(st => st.IsDeleted != true)
            .Where(st => st.TaskId == query.TaskId)
            .ToListAsync(cancellationToken);

        var subTaskDtos = new List<SubTaskListItemDto>();

        foreach (var subTaskEntity in subTaskEntities)
        {
            subTaskDtos.Add(SubTaskListItemHelper.ToListItem(subTaskEntity));
        }

        var taskDto = new TaskListItemDto(
            query.TaskId,
            taskEntity.ReferenceNo ?? string.Empty,
            taskEntity.Name,
            taskEntity.UpdatedBy,
            taskEntity.DateUpdated
        );

        var tasksWithSubtasks = new TaskListItemWithSubTaskList(taskDto, subTaskDtos);

        return Result.Success(new TaskDetailsViewModel(tasksWithSubtasks, query.TabIndex, dateTimeProvider));
    }
}
