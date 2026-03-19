namespace SchoolAccount.Application.Features.Tasks.Search.Queries.GetPage;

public sealed record TaskListItemDto(long Id, string ReferenceNo, string Name, string UpdatedBy, DateTime DateUpdated);
