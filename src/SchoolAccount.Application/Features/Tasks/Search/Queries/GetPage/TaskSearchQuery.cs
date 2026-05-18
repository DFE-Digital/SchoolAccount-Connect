using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskSearchQuery(
    string? Term,
    int PageNumber = 1,
    int PageSize = 10
) : IQuery<TaskSearchResponse>;
