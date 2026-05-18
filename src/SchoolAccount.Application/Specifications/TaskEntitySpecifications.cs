using System.Linq.Expressions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Specifications;

public static class TaskEntitySpecifications
{
    public static Expression<Func<TaskEntity, bool>> IsAccessibleForSchoolType(
        IQueryable<SchoolTypeTagMappingEntity> schoolTypeMappings,
        IEnumerable<SchoolType> types
    )
    {
        var typeIds = types.Select(t => (int)t).ToList();

        return t =>
            t.SubTasks.Any(sub =>
                sub.TagsSourceMappings.Any(tsm =>
                    schoolTypeMappings.Any(st =>typeIds.Contains(st.SchoolTypeId) && st.TagId == tsm.TagId)
                )
            );
    }

    public static Expression<Func<TaskEntity, bool>> IsVisible()
    {
        return t =>
            t.SubTasks.Any(sub =>
                sub.WorkflowState == WorkflowState.Published || sub.WorkflowState == WorkflowState.Expired
            );
    }
}
