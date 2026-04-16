using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Category.Query;

public class GetAllParentCategoriesQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetAllParentCategoriesQuery, CategoryPagedResult>
{
    public async Task<Result<CategoryPagedResult>> Handle(
        GetAllParentCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        var result = await applicationDbContext
            .Types.AsNoTracking()
            .Where(x => x.TypeGrouping != null && x.TypeGrouping.Id == 1)
            .Select(x => new CategoryRow
            {
                Id = x.Id,
                Name = x.DisplayName,
                Description = x.Description,
            })
            .PaginateAsync(query.PageSize, query.PageNumber, cancellationToken);

        return Result.Success(new CategoryPagedResult(result));
    }
}
