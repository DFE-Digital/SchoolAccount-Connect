using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public static class GetTaskByIdErrors
{
    public static Error NotFound(long taskId) =>
        Error.NotFound("Task.NotFound", $"The task with the Id = '{taskId}' was not found");
}
