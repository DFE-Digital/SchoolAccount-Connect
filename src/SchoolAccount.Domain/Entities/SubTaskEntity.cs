using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Enums;

namespace SchoolAccount.Domain.Entities;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SubTaskEntity
{
    public long Id { get; set; }

    public long TaskId { get; set; }

    public required string Name { get; set; }

    public string? ReferenceNo { get; set; }

    public string? Description { get; set; }

    public string? DigitalTaskLink { get; set; }

    public Requirement Requirement { get; set; }

    public DateOnly? StartDate { get; set; }

    public bool? StartDateIsExact { get; set; }

    public DateOnly? DueDate { get; set; }

    public bool? DueDateIsExact { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public WorkflowState WorkflowState { get; set; }

    public string? Comment { get; set; }

    public int? Version { get; set; }

    public bool? IsDeleted { get; set; }

    public string? DisplayDate { get; set; }

    public string? ArchiveComment { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public required string UpdatedBy { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual required TaskEntity Task { get; set; }

    public virtual ICollection<TagsSourceMappingEntity> TagsSourceMappings { get; } = [];
}
