using Microsoft.EntityFrameworkCore;
using SchoolAccount.Application.Abstractions.Data;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Extensions;
using SchoolAccount.Application.Specifications;
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
            .Select(GetCategoryHubProjection.ToCategoryHubResponseCategory())
            .FirstOrDefaultAsync(cancellationToken);

        if (category is null)
        {
            return Result.Failure<GetCategoryHubResponse>(GetCategoryHubErrors.NotFound(query.Id));
        }

        var tasks = await applicationDbContext
            .Tasks.AsNoTracking()
            .Where(TaskEntitySpecifications.InCategory(query.Id))
            .Where(TaskEntitySpecifications.IsVisible())
            .AsSingleQuery()
            .Select(GetCategoryHubProjection.ToCategoryHubResponseTasks())
            .PaginateAsync(query.PageSize, query.PageNumber, cancellationToken);

        return Result.Success(new GetCategoryHubResponse(category, tasks));
    }
}
