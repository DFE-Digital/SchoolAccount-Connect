using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Sources;
using SchoolAccount.Domain.Subtasks;

namespace SchoolAccount.Domain.Tags;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class TagsSourceMappingEntity
{
    public long Id { get; set; }

    public long EntityId { get; set; }

    public int SourceId { get; set; }

    public long TagId { get; set; }

    public virtual required SubTaskEntity SubTask { get; set; }

    public virtual required SourceEntity Source { get; set; }

    public virtual required TagEntity Tag { get; set; }
}
