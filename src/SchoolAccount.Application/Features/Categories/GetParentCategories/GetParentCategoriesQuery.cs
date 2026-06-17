using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Application.Features.Categories.GetParentCategories;

public sealed record GetParentCategoriesQuery(int PageNumber, int PageSize) : IQuery<GetParentCategoriesResponse>;
