namespace SchoolAccount.Domain.Common;

public enum WorkflowState
{
    None = 0,
    Draft,
    Queued,
    Published,
    Expired,
    Archived,
}
