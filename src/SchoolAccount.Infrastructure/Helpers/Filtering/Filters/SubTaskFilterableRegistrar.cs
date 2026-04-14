using System.Linq.Expressions;
using SchoolAccount.Application.Features.CalendarOfItems.Models;
using SchoolAccount.Application.Features.Shared.Filtering;
using SchoolAccount.Domain.Entities;
using SchoolAccount.Domain.Enums;
using SchoolAccount.Infrastructure.Helpers.Filtering.Interfaces;
using Type = System.Type;

namespace SchoolAccount.Infrastructure.Helpers.Filtering.Filters;

public class SubTaskFilterableRegistrar : IFilterableRegistrar<CalendarOfItemsFilter>
{
    public static class Keys
    {
        public const string Name = "name";
        public const string Categories = "category";
        public const string State = "state";
        public const string PhaseOfEducation = "phaseOfEducation";
    }

    public Type TypeBeingRegistered => typeof(SubTaskEntity);

    public FieldSelector FieldSelectorsBeingRegistered => new()
    {
        [Keys.Name] = (Expression<Func<SubTaskEntity, string>>)(x => x.Name),
        [Keys.Categories] =
            (Expression<Func<SubTaskEntity, IEnumerable<int>>>)(x => x.Task.TypeTaskMappings.Select(x => x.TypeId)),
            [Keys.State] = (Expression<Func<SubTaskEntity, WorkflowState>>)(x => x.WorkflowState),
        [Keys.PhaseOfEducation] =
            (Expression<Func<SubTaskEntity, IEnumerable<long>>>)(x => x.TagsSourceMappings.Select(x => x.TagId)),
    };

    public void ConsolidateFilters(CalendarOfItemsFilter filter)
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
