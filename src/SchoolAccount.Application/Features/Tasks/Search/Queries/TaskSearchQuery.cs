using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Tasks.Search.Queries;

public sealed record TaskSearchQuery(string Term) : IQuery<TaskSearchResultsVm>;