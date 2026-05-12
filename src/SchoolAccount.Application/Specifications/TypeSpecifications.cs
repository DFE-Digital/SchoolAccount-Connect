using System.Linq.Expressions;
using SchoolAccount.Domain.Types;

namespace SchoolAccount.Application.Specifications;

public static class TypeSpecifications
{
    public static Expression<Func<TypeEntity, bool>> OnlyActiveHubTypes()
    {
        return x => x.TypeGrouping != null && x.TypeGrouping.Id == 1;
    }

    public static Expression<Func<TypeEntity, bool>> TopLevelOnly()
    {
        return x => x.ParentTypeId == null;
    }

    public static Expression<Func<TypeEntity, bool>> HasAssociatedTasks()
    {
        return x => x.TypeTaskMappings.Any();
    }
}
