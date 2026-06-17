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
        SchoolType type
    )
    {
        if (type <= 0)
        {
            return s => true;
        }

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

    public static Expression<Func<SubTaskEntity, bool>> IsPublished()
    {
        return s => s.WorkflowState == WorkflowState.Published;
    }

    public static Expression<Func<SubTaskEntity, bool>> HasDate()
    {
        return s => s.DueDate != null || s.StartDate != null;
    }

    public static Expression<Func<SubTaskEntity, bool>> WithinDateRange(DateOnly start, DateOnly end)
    {
        return s => (s.DueDate ?? s.StartDate) >= start && (s.DueDate ?? s.StartDate) <= end;
    }
}
