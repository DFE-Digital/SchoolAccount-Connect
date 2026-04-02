using SchoolAccount.Domain.Workflow;

namespace SchoolAccount.Domain.Dtos;

public record SubTaskListItemDto(
    long Id,
    string ReferenceNo,
    string Name,
    string Description,
    string? DigitalLink,
    string UpdatedBy,
    DateOnly? StartDate,
    DateTime DateUpdated,
    DateOnly? DueDate,
    int? RequirementId,
    bool? StartDateExact,
    bool? DueDateIsExact,
    WorkflowState WorkflowState
)
{
    public bool IsPublishedAndHasStartAndEndDate => _hasStartAndDueDate && WorkflowState == WorkflowState.Published;
    public bool IsExpiredAndHasStartAndEndDate => _hasStartAndDueDate && WorkflowState == WorkflowState.Expired;
    private bool _hasStartAndDueDate => !(StartDate == null && DueDate == null);
}
