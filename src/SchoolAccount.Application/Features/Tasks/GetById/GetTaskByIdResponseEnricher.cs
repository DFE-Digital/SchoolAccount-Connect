using SchoolAccount.Application.Common;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed class GetTaskByIdResponseEnricher(IDateTimeProvider dateTimeProvider)
{
    private readonly AvailabilityLabel _availabilityLabel = new(dateTimeProvider);

    public GetTaskByIdResponse Enrich(GetTaskByIdResponse getTaskByIdResponse)
    {
        return getTaskByIdResponse with { SubTasks = getTaskByIdResponse.SubTasks.Select(EnrichSubTask).ToArray() };
    }

    private GetTaskByIdResponseSubtask EnrichSubTask(GetTaskByIdResponseSubtask subtask)
    {
        return subtask with
        {
            AvailabilityLabel = _availabilityLabel.Generate(
                subtask.WorkflowState,
                subtask.StartDate,
                subtask.StartDateIsExact,
                subtask.DueDate
            ),
            DueDateLabel = Common.DueDateLabel.Generate(subtask.DueDate, subtask.DueDateIsExact),
        };
    }
}
