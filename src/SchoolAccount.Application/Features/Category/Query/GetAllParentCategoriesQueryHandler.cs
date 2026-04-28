using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Category.Contracts;
using SchoolAccount.Application.Features.Category.Models;
using SchoolAccount.Application.Specifications;
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
        var extraCategory = new CategoryRow { Name = "All tasks" };

        var dbCategories = await applicationDbContext
            .Types.AsNoTracking()
            .Where(TypeSpecifications.OnlyActiveHubTypes())
            .Select(x => new CategoryRow
            {
                Id = x.Id,
                Name = x.DisplayName,
                Description = x.Description,
            })
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);

        var result = new[] { extraCategory }
            .Concat(dbCategories)
            .PaginateForExtraItem(query.PageSize, query.PageNumber);

        return Result.Success(new CategoryPagedResult(result));
    }
}
