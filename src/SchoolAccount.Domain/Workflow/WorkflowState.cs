namespace SchoolAccount.Domain.Workflow;

public enum WorkflowState
{
    None = 0,
    Draft,
    Queued,
    Published,
    Expired,
    Archived,
}
