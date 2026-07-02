using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Specifications;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Categories.GetParentCategories;

public sealed class GetParentCategoriesQueryHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetParentCategoriesQuery, GetParentCategoriesResponse>
{
    public async Task<Result<GetParentCategoriesResponse>> Handle(
        GetParentCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        var baseQuery = applicationDbContext
            .Types.AsNoTracking()
            .Where(TypeEntitySpecifications.OnlyActiveHubTypes())
            .Where(TypeEntitySpecifications.TopLevelOnly())
            .Where(TypeEntitySpecifications.HasAssociatedTasks());

        var categories = await baseQuery
            .OrderBy(category => category.Name)
            .Select(GetParentCategoriesProjection.ParentCategories())
            .PaginateAsync(query.PageSize, query.PageNumber, cancellationToken);

        return Result.Success(new GetParentCategoriesResponse { Categories = categories });
    }
}
