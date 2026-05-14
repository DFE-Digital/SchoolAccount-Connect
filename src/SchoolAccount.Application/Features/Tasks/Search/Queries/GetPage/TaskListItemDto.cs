namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskListItemDto(long Id, string Name, string Description, DateTime DateUpdated);