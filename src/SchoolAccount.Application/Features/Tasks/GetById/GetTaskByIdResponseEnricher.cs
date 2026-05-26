using SchoolAccount.Application.Features.Tasks.Common.Labels;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed class GetTaskByIdResponseEnricher(IDateTimeProvider dateTimeProvider)
{
    private readonly SubTaskAvailabilityLabel _subTaskAvailabilityLabel = new(dateTimeProvider);

    public GetTaskByIdResponse Enrich(GetTaskByIdResponse getTaskByIdResponse)
    {
        return getTaskByIdResponse with { SubTasks = getTaskByIdResponse.SubTasks.Select(EnrichSubTask).ToArray() };
    }

    private GetTaskByIdResponseSubtask EnrichSubTask(GetTaskByIdResponseSubtask subtask)
    {
        return subtask with
        {
            AvailabilityLabel = _subTaskAvailabilityLabel.Generate(
                subtask.WorkflowState,
                subtask.StartDate,
                subtask.StartDateIsExact,
                subtask.DueDate
            ),
            DueDateLabel = SubTaskDueDateLabel.Generate(subtask.DueDate, subtask.DueDateIsExact),
        };
    }
}
