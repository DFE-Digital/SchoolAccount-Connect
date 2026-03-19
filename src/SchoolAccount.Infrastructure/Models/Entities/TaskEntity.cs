namespace SchoolAccount.Infrastructure.Models.Entities;

public class TaskEntity : AuditableDatabaseEntity
{
    public string Name { get; set; } = null!;

    public string? ReferenceNo { get; set; }
    public string? Description { get; set; }

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

    public virtual ICollection<SubTaskEntity> SubTasks { get; } = [];
}
