using SchoolAccount.Application.Features.Tasks.GetById;

namespace SchoolAccount.Tests.Common.Builders.Tasks.GetById;

public class GetTaskByIdResponseBuilder
{
    private List<GetTaskByIdResponseSubtask> _subtasks = [];

    public static GetTaskByIdResponseBuilder AResponse() => new();

    public GetTaskByIdResponseBuilder WithSubtasks(params GetTaskByIdResponseSubtask[] subtasks)
    {
        _subtasks = [.. subtasks];
        return this;
    }

    public GetTaskByIdResponse Build() => new() { SubTasks = [.. _subtasks] };

    public static implicit operator GetTaskByIdResponse(GetTaskByIdResponseBuilder builder) => builder.Build();
}
