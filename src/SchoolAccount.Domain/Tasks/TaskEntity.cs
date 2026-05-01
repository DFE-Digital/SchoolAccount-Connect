using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Types;
using static SchoolAccount.Domain.Common.WorkflowState;

namespace SchoolAccount.Domain.Tasks;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TaskEntity
{
    public long Id { get; init; }

    public required string Name { get; set; }

    public string? ReferenceNo { get; set; }

    public string? Description { get; set; }

    public long? ServiceId { get; set; }

    public DateTime? PublishDate { get; set; }

    public Requirement? Requirement { get; set; }

    public WorkflowState WorkflowState { get; set; }

    public bool? IsDeleted { get; set; }

    public string? PublishComment { get; set; }

    public string? ArchiveComment { get; set; }

    public long? TeamId { get; set; }

    public int? Version { get; set; }

    public bool IsLatestVersion { get; set; }

    public required string CreatedBy { get; set; }

    public DateTime DateCreated { get; set; }

    public required string UpdatedBy { get; set; }

    public DateTime DateUpdated { get; set; }

    public virtual ICollection<SubTaskEntity> SubTasks { get; } = [];

    public virtual ICollection<TypeTaskMappingEntity> TypeTaskMappings { get; } = [];

    public virtual ICollection<ResourceEntity> Resources { get; } = [];

    [NotMapped]
    public DateTime? SubTaskLastUpdated =>
        SubTasks.OrderByDescending(st => st.DateUpdated).FirstOrDefault()?.DateUpdated;

    [NotMapped]
    public IEnumerable<SubTaskEntity> ExpiredSubTasks =>
        SubTasks.Where(subtask => subtask.WorkflowState == Expired).OrderByDescending(st => st.SortingDate);

    [NotMapped]
    public IEnumerable<SubTaskEntity> PublishedSubTasks =>
        SubTasks.Where(subtask => subtask.WorkflowState == Published).OrderByDescending(subtask => subtask.SortingDate);
}
