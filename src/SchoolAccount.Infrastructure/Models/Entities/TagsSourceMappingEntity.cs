using System.ComponentModel.DataAnnotations.Schema;
using SchoolAccount.Infrastructure.Models.Interfaces;

namespace SchoolAccount.Infrastructure.Models.Entities;

public class TagsSourceMappingEntity : IDatabaseEntity
{
    public long Id { get; set; }

    public long EntityId { get; set; }

    [NotMapped]
    public virtual IDatabaseEntity? Entity { get; set; }

    public int SourceId { get; set; }

    public long TagId { get; set; }

    public virtual SourceEntity Source { get; } = null!;

    public virtual TagEntity Tag { get; } = null!;
}
