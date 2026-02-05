namespace SchoolAccount.Infrastructure.Models;

public class TaskEntity : AuditableDatabaseEntity
{

    public string? TaskReferenceNo { get; set; }
    public string? TaskName { get; set; }
    public string? TaskDescription { get; set; }

    public long? ServiceId { get; set; }
    public DateTime? PublishDate { get; set; }
    public long? RequirementId { get; set; }
    public long? WorkflowStateId { get; set; }

    public bool? IsDeleted { get; set; }

    public string? PublishComment { get; set; }
    public string? ArchiveComment { get; set; }

    public long? TeamId { get; set; }

    public int? Version { get; set; }
    public bool IsLatestVersion { get; set; }
}