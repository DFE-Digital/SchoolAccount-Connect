using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Domain.Dtos;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskSearchQuery(string Term) : IQuery<TaskWithSubTasksDto>;
