using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Features.Categories.GetCategoryHub;

public sealed class GetCategoryHubHandler(IApplicationDbContext applicationDbContext)
    : IQueryHandler<GetCategoryHubQuery, GetCategoryHubResponse>
{
    public async Task<Result<GetCategoryHubResponse>> Handle(
        GetCategoryHubQuery query,
        CancellationToken cancellationToken
    )
    {
        var category = await applicationDbContext
            .Types.AsNoTracking()
            .Where(category => category.Id == query.Id)
            .AsSingleQuery()
            .Select(GetCategoryHubProjection.ToCategoryHubResponse(query.PageNumber, query.PageSize))
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result.Failure<GetCategoryHubResponse>(GetCategoryHubErrors.NotFound(query.Id));
        }

        return Result.Success(category);
    }
}
