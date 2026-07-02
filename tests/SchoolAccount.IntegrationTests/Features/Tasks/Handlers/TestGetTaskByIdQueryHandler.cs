using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Tasks.Handlers;

public class TestGetTaskByIdQueryHandler : IQueryHandler<GetTaskByIdQuery, GetTaskByIdResponse>
{
    public GetTaskByIdResponse Response { get; set; } = DefaultResponse();

    public async Task<Result<GetTaskByIdResponse>> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        return await Task.FromResult(Result.Success(Response));
    }

    public void Clear() => Response = DefaultResponse();

    // The controller reads TaskTypes.First() for the breadcrumb, so a valid
    // response always needs at least one task type
    public static GetTaskByIdResponse DefaultResponse() =>
        new()
        {
            Id = 1,
            Name = "School attendance",
            TaskTypes = [new GetTaskByIdResponseTaskType { Id = 5, Name = "Pupils and attendance" }],
        };
}
