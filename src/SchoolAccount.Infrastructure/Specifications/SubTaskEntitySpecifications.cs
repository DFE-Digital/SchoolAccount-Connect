using System.Linq.Expressions;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Domain.Workflow;
using SchoolAccount.Kernel;
using RequirementEntity = SchoolAccount.Domain.Entities.RequirementEntity;

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
        return s => s.RequirementId == RequirementEntity.IdValues.Mandatory;
    }

    public static Expression<Func<SubTaskEntity, bool>> IsVisible()
    {
        return s => s.WorkflowState == WorkflowState.Published || s.WorkflowState == WorkflowState.Expired;
    }
}
