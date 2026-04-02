namespace SchoolAccount.Domain.Entities;

public class SubTaskEntity : AuditableDatabaseEntity
{
    public string Name { get; set; } = null!;
    public long TaskId { get; set; }

    public string? ReferenceNo { get; set; }
    public string? Description { get; set; }

    public string? DigitalTaskLink { get; set; }

    public int? RequirementId { get; set; }

    public DateOnly? StartDate { get; set; }
    public bool? StartDateIsExact { get; set; }

    public DateOnly? DueDate { get; set; }
    public bool? DueDateIsExact { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public int WorkflowStateId { get; set; }

    public string? Comment { get; set; }

    public int? Version { get; set; }
    public bool? IsDeleted { get; set; }

    public string? DisplayDate { get; set; }

    public string? ArchiveComment { get; set; }

    public virtual TaskEntity Task { get; set; } = null!;
    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = null!;
    public virtual WorkflowStateEntity WorkflowState { get; set; } = null!;
}
