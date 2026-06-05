using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Features.Categories.Contracts;
using SchoolAccount.Application.Features.Categories.Models;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Categories.Query;

public class GetAllParentCategoriesThatHaveAssociatedTasksQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetAllParentCategoriesThatHaveAssociatedTasksQuery, CategoryPagedResult>
{
    public async Task<Result<CategoryPagedResult>> Handle(
        GetAllParentCategoriesThatHaveAssociatedTasksQuery thatHaveAssociatedTasksQuery,
        CancellationToken cancellationToken
    )
    {
        var extraCategory = new CategoryRow { Name = "All tasks" };

        var dbCategories = await applicationDbContext
            .Types.AsNoTracking()
            .Where(TypeSpecifications.OnlyActiveHubTypes())
            .Where(TypeSpecifications.HasAssociatedTasks())
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
            .PaginateForExtraItem(thatHaveAssociatedTasksQuery.PageSize, thatHaveAssociatedTasksQuery.PageNumber);

        return Result.Success(new CategoryPagedResult(result));
    }
}
