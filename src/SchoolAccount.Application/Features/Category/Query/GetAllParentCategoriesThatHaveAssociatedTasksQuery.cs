using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Features.Category.Contracts;

namespace SchoolAccount.Application.Features.Category.Query;

public record GetAllParentCategoriesThatHaveAssociatedTasksQuery(int PageNumber = 1, int PageSize = 10)
    : IQuery<CategoryPagedResult>;
