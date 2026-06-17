using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Domain.Common;

namespace SchoolAccount.Tests.Common.Builders.Tasks.GetById;

public class GetTaskByIdResponseSubtaskBuilder
{
    private int _id;
    private string _name = "SubTask";
    private WorkflowState _workflowState = WorkflowState.Published;
    private DateOnly? _dueDate;
    private DateOnly? _startDate;
    private DateTime _dateUpdated = new(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    public static GetTaskByIdResponseSubtaskBuilder ASubtask() => new();

    public GetTaskByIdResponseSubtaskBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder Published()
    {
        _workflowState = WorkflowState.Published;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder Expired()
    {
        _workflowState = WorkflowState.Expired;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder WithDueDate(DateOnly dueDate)
    {
        _dueDate = dueDate;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder WithStartDate(DateOnly startDate)
    {
        _startDate = startDate;
        return this;
    }

    public GetTaskByIdResponseSubtaskBuilder WithDateUpdated(DateTime dateUpdated)
    {
        _dateUpdated = dateUpdated;
        return this;
    }

    public GetTaskByIdResponseSubtask Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            WorkflowState = _workflowState,
            DueDate = _dueDate,
            StartDate = _startDate,
            DateUpdated = _dateUpdated,
        };

    public static implicit operator GetTaskByIdResponseSubtask(GetTaskByIdResponseSubtaskBuilder builder) =>
        builder.Build();
}
