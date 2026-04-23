using System.Diagnostics.CodeAnalysis;
using SchoolAccount.Domain.Tags;

namespace SchoolAccount.Domain.SchoolTypes;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public class SchoolTypeTagMappingEntity
{
    public int Id { get; set; }

    public int SchoolTypeId { get; set; }

    public long TagId { get; set; }

    public virtual required SchoolTypeEntity SchoolType { get; set; }

    public virtual required TagEntity Tag { get; set; }
}
