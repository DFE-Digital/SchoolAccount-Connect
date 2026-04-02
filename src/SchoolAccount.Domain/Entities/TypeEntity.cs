namespace SchoolAccount.Domain.Entities;

public class TypeEntity
{
    public int Id { get; set; }

    public string TagName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentTypeId { get; set; }

    public int? TypeGroupingId { get; set; }

    public virtual TypeGroupingEntity? TypeGrouping { get; set; }

    public virtual ICollection<TypeTaskMappingEntity> TypeTaskMappings { get; } = [];
}
