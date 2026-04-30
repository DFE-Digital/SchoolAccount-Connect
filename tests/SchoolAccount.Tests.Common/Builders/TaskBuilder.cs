using SchoolAccount.Domain.Common;
using SchoolAccount.Domain.Resources;
using SchoolAccount.Domain.Subtasks;
using SchoolAccount.Domain.Tasks;

namespace SchoolAccount.Tests.Common.Builders;

public sealed class TaskBuilder
{
    private long _id = 1;
    private readonly List<SubTaskEntity> _subTasks = [];
    private readonly List<ResourceEntity> _resources = [];
    private string _name = "Test Task";
    private string? _referenceNo;
    private Requirement _requirement = Requirement.Mandatory;
    private DateTime _dateUpdated = DateTime.UtcNow;
    private string _updatedBy = "tester";

    public static TaskBuilder ATask() => new();

    public TaskBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public TaskBuilder Named(string name)
    {
        _name = name;
        return this;
    }

    public TaskBuilder WithReferenceNo(string? referenceNo)
    {
        _referenceNo = referenceNo;
        return this;
    }

    public TaskBuilder WithRequirement(Requirement requirement)
    {
        _requirement = requirement;
        return this;
    }

    public TaskBuilder UpdatedBy(string updatedBy)
    {
        _updatedBy = updatedBy;
        return this;
    }

    public TaskBuilder UpdatedAt(int year, int month, int day, int hour = 0, int minute = 0, int second = 0)
    {
        _dateUpdated = new DateTime(year, month, day, hour, minute, second);
        return this;
    }

    public TaskBuilder WithSubTask(SubTaskBuilder builder)
    {
        _subTasks.Add(builder.Build());
        return this;
    }

    public TaskBuilder WithSubTasks(params SubTaskBuilder[] builders)
    {
        foreach (var b in builders)
        {
            _subTasks.Add(b.Build());
        }

        return this;
    }

    public TaskBuilder WithResources(params ResourceBuilder[] builders)
    {
        foreach (var builder in builders)
        {
            _resources.Add(builder.Build());
        }

        return this;
    }

    public TaskEntity Build()
    {
        var task = new TaskEntity
        {
            Id = _id,
            Name = _name,
            ReferenceNo = _referenceNo,
            Requirement = _requirement,
            CreatedBy = "tester",
            UpdatedBy = _updatedBy,
            DateUpdated = _dateUpdated,
        };

        foreach (var subtask in _subTasks)
        {
            task.SubTasks.Add(subtask);
        }

        foreach (var resource in _resources)
        {
            task.Resources.Add(resource);
        }

        return task;
    }
}
