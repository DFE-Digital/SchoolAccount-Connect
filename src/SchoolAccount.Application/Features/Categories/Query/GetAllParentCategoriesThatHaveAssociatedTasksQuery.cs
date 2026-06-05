using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Categories.Contracts;

namespace SchoolAccount.Application.Features.Categories.Query;

public record GetAllParentCategoriesThatHaveAssociatedTasksQuery(int PageNumber = 1, int PageSize = 10)
    : IQuery<CategoryPagedResult>;
