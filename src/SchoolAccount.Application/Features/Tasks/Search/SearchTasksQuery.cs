using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.Search;

public sealed record SearchTasksQuery(string? Term, int PageNumber, int PageSize) : IQuery<SearchTasksResponse>;
