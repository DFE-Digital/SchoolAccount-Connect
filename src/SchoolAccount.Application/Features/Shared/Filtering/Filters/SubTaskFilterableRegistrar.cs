using System.Linq.Expressions;
using SchoolAccount.Application.Features.Shared.Filtering.Interfaces;
using SchoolAccount.Application.Features.Shared.Filtering.Models;
using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Subtasks;
using Type = System.Type;

namespace SchoolAccount.Application.Features.Shared.Filtering.Filters;

public class SubTaskFilterableRegistrar : IFilterableAndConsolidateRegistrar
{
    public static class Keys
    {
        public const string Name = "name";
        public const string Categories = "category";
        public const string State = "state";
        public const string PhaseOfEducation = "phaseOfEducation";
    }

    public Type TypeBeingRegistered => typeof(SubTaskEntity);

    public FieldSelector FieldSelectorsBeingRegistered =>
        new()
        {
            [Keys.Name] = (Expression<Func<SubTaskEntity, string>>)(x => x.Name),
            [Keys.Categories] =
                (Expression<Func<SubTaskEntity, IEnumerable<int>>>)(x => x.Task.TypeTaskMappings.Select(t => t.TypeId)),
            [Keys.State] = (Expression<Func<SubTaskEntity, WorkflowState>>)(x => x.WorkflowState),
            [Keys.PhaseOfEducation] =
                (Expression<Func<SubTaskEntity, IEnumerable<long>>>)(x => x.TagsSourceMappings.Select(t => t.TagId)),
        };

    public void ConsolidateFilters(IList<FilterRequest> filter)
    {
        if (filter.All(x => x.Field != Keys.State))
        {
            filter.Add(
                new()
                {
                    Field = Keys.State,
                    Operator = ComparisonType.In,
                    Value = new List<WorkflowState> { WorkflowState.Published, WorkflowState.Expired },
                }
            );
        }
    }
}
