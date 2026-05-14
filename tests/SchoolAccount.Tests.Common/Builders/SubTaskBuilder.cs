using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class SubTaskBuilder
{
    private long _id = 1;
    private string _name = "Sub Task";
    private string? _referenceNo;
    private string? _description;
    private Requirement _requirement = Requirement.Mandatory;
    private WorkflowState _state = WorkflowState.Draft;
    private DateTime _dateUpdated = DateTime.UtcNow;
    private string _updatedBy = "tester";
    private DateOnly? _startDate;
    private bool? _startDateIsExact;
    private DateOnly? _dueDate;
    private bool? _dueDateIsExact;
    private readonly List<ResourceEntity> _resources = [];

    public static SubTaskBuilder ASubTask() => new();

    public SubTaskBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public SubTaskBuilder Named(string name)
    {
        _name = name;
        return this;
    }

    public SubTaskBuilder WithReferenceNo(string? referenceNo)
    {
        _referenceNo = referenceNo;
        return this;
    }

    public SubTaskBuilder WithDescription(string? description)
    {
        _description = description;
        return this;
    }

    public SubTaskBuilder WithRequirement(Requirement requirement)
    {
        _requirement = requirement;
        return this;
    }

    public SubTaskBuilder InState(WorkflowState state)
    {
        _state = state;
        return this;
    }

    public SubTaskBuilder UpdatedAt(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
    {
        _dateUpdated = new DateTime(year, month, day, hour, minute, second, DateTimeKind.Utc);
        return this;
    }

    public SubTaskBuilder UpdatedAt(DateTime dateUpdated)
    {
        _dateUpdated = dateUpdated;
        return this;
    }

    public SubTaskBuilder UpdatedBy(string updatedBy)
    {
        _updatedBy = updatedBy;
        return this;
    }

    public SubTaskBuilder WithStartDate(int year, int month, int day, bool? isExact = true)
    {
        _startDate = new DateOnly(year, month, day);
        _startDateIsExact = isExact;
        return this;
    }

    public SubTaskBuilder WithDueDate(int year, int month, int day, bool? isExact = true)
    {
        _dueDate = new DateOnly(year, month, day);
        _dueDateIsExact = isExact;
        return this;
    }

    public SubTaskBuilder WithResources(params ResourceBuilder[] builders)
    {
        foreach (var builder in builders)
        {
            _resources.Add(builder.Build());
        }

        return this;
    }

    public SubTaskEntity Build()
    {
        var subtask = new SubTaskEntity
        {
            Id = _id,
            Name = _name,
            ReferenceNo = _referenceNo,
            Description = _description,
            Requirement = _requirement,
            CreatedBy = "tester",
            UpdatedBy = _updatedBy,
            WorkflowState = _state,
            DateUpdated = _dateUpdated,
            StartDate = _startDate,
            StartDateIsExact = _startDateIsExact,
            DueDate = _dueDate,
            DueDateIsExact = _dueDateIsExact,
            Task = new TaskEntity
            {
                Name = "Stub",
                CreatedBy = "tester",
                UpdatedBy = "tester",
            },
        };

        foreach (var resource in _resources)
        {
            subtask.Resources.Add(resource);
        }

        return subtask;
    }
}
