namespace SchoolAccount.Infrastructure.Models;

public sealed class SubTaskEntity : AuditableDatabaseEntity
{
    public long TaskId { get; set; }

    public long? ServiceId { get; set; }

    public string? SubTaskReferenceNo { get; set; }
    public string? SubTaskName { get; set; }
    public string? SubTaskDescription { get; set; }

    public string? DigitalTaskLink { get; set; }

    public long? RequirementId { get; set; }

    public DateTime? StartDate { get; set; }
    public bool StartDateIsExact { get; set; }

    public DateTime? DueDate { get; set; }
    public bool DueDateIsExact { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public long? WorkflowStateId { get; set; }

    public string? Comment { get; set; }

    public int? Version { get; set; }
    public bool? IsDeleted { get; set; }

    public DateTime? DisplayDate { get; set; }

    public string? ArchiveComment { get; set; }

    public long? TeamId { get; set; }
}