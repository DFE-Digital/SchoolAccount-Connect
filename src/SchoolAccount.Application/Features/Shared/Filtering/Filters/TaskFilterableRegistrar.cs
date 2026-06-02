using System.Linq.Expressions;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Tasks;
using Type = System.Type;

namespace SchoolAccount.Application.Features.Shared.Filtering.Filters;

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
