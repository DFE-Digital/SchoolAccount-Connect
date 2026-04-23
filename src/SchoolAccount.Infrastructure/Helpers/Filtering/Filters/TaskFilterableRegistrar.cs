using System.Linq.Expressions;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Tasks;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using Type = System.Type;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Filters;

public class TaskFilterableRegistrar : IFilterableRegistrar
{
    public static class Keys
    {
        public const string Categories = "category";
        public const string State = "state";
    }

    public Type TypeBeingRegistered => typeof(TaskEntity);

    public FieldSelector FieldSelectorsBeingRegistered =>
        new()
        {
            [Keys.Categories] =
                (Expression<Func<TaskEntity, IEnumerable<int>>>)(x => x.TypeTaskMappings.Select(ttm => ttm.TypeId)),
            [Keys.State] = (Expression<Func<TaskEntity, WorkflowState>>)(x => x.WorkflowState),
        };
}
