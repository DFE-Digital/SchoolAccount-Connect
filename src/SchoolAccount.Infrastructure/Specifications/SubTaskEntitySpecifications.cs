using System.Linq.Expressions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Infrastructure.Specifications;

public static class SubTaskEntitySpecifications
{
    public static Expression<Func<SubTaskEntity, bool>> IsAccessibleForSchoolType(
        IQueryable<SchoolTypeTagMappingEntity> schoolTypeMappings,
        SchoolType type
    )
    {
        return s =>
            s.TagsSourceMappings.Any(t =>
                schoolTypeMappings.Any(st => st.SchoolTypeId == (int)type && st.TagId == t.TagId)
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
