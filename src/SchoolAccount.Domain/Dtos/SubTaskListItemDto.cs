using SchoolAccount.Domain.ViewModels;

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
    int WorkflowStateId
)
{
    public bool IsPublishedAndHasStartAndEndDate =>
        _hasStartAndDueDate && WorkflowStateId == (long)WorkflowStateValues.Published;
    public bool IsExpiredAndHasStartAndEndDate =>
        _hasStartAndDueDate && WorkflowStateId == (long)WorkflowStateValues.Expired;
    private bool _hasStartAndDueDate => !(StartDate == null && DueDate == null);
}
