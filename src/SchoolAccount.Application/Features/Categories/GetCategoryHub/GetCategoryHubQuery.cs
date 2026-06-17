using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public sealed record GetCategoryHubQuery(int Id, int PageNumber, int PageSize) : IQuery<GetCategoryHubResponse>;
