using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed record GetAllTasksQuery(int PageNumber, int PageSize) : IQuery<GetAllTasksResponse>;
