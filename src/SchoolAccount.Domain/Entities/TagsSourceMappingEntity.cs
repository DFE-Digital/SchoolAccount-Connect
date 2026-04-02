namespace SchoolAccount.Domain.Entities;

public class TagsSourceMappingEntity
{
    public long Id { get; set; }

    public long EntityId { get; set; }

    public int SourceId { get; set; }

    public long TagId { get; set; }

    public virtual SubTaskEntity SubTask { get; } = null!;

    public virtual SourceEntity Source { get; } = null!;

    public virtual TagEntity Tag { get; } = null!;
}
