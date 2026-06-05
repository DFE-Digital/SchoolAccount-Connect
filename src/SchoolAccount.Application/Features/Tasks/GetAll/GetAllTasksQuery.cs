using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.GetAll;

public sealed record GetAllTasksQuery(int PageNumber = 1, int PageSize = 10) : IQuery<GetAllTasksResponse>;
