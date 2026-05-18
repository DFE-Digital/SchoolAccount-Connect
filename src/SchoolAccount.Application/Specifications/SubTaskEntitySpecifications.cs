using System.Linq.Expressions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Specifications;

public static class SubTaskEntitySpecifications
{
    public static Expression<Func<SubTaskEntity, bool>> IsAccessibleForSchoolType(
        IQueryable<SchoolTypeTagMappingEntity> schoolTypeMappings,
        IEnumerable<SchoolType> types
    )
    {
        var typeIds = types.Select(t => (int)t).ToList();

        return s =>
            s.TagsSourceMappings.Any(t =>
                schoolTypeMappings.Any(st => typeIds.Contains(st.SchoolTypeId) && st.TagId == t.TagId)
            );
    }

    public static Expression<Func<SubTaskEntity, bool>> IsMandatory()
    {
        return s => s.Requirement == Requirement.Mandatory;
    }

    public static Expression<Func<SubTaskEntity, bool>> IsVisible()
    {
        return s => s.WorkflowState == WorkflowState.Published || s.WorkflowState == WorkflowState.Expired;
    }
}
