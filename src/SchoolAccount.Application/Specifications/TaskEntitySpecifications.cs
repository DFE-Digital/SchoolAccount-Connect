using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.SchoolTypes;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Specifications;

public static class TaskEntitySpecifications
{
    public static Expression<Func<TaskEntity, bool>> IsAccessibleForSchoolType(
        IQueryable<SchoolTypeTagMappingEntity> schoolTypeMappings,
        SchoolType type
    )
    {
        if (type <= 0)
        {
            return t => true;
        }

        return t =>
            t.SubTasks.Any(sub =>
                sub.TagsSourceMappings.Any(tsm =>
                    schoolTypeMappings.Any(st => st.SchoolTypeId == (int)type && st.TagId == tsm.TagId)
                )
            );
    }

    public static Expression<Func<TaskEntity, bool>> HasVisibleSubTasks()
    {
        return t =>
            t.SubTasks.Any(sub =>
                sub.WorkflowState == WorkflowState.Published || sub.WorkflowState == WorkflowState.Expired
            );
    }

    public static Expression<Func<TaskEntity, bool>> IsVisible()
    {
        return t => t.WorkflowState == WorkflowState.Published || t.WorkflowState == WorkflowState.Expired;
    }

    public static Expression<Func<TaskEntity, bool>> InCategory(int categoryId)
    {
        return task => task.Types.Any(type => type.Id == categoryId);
    }

    public static Expression<Func<TaskEntity, bool>> ContainsTerm(string term)
    {
        return task => EF.Functions.Like(task.Name, term) || EF.Functions.Like(task.Description ?? string.Empty, term);
    }
}
