using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Tasks.GetById;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskSearchQuery(string Term) : IQuery<TaskWithSubTasksDto>;
