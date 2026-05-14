using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.GetById;

public sealed record GetTaskByIdQuery(long Id) : IQuery<GetTaskByIdResponse>;
