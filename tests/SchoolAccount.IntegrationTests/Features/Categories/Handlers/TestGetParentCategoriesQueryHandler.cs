using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Categories.GetParentCategories;
using SchoolAccount.Kernel;

namespace SchoolAccount.IntegrationTests.Features.Categories.Handlers;

public class TestGetParentCategoriesQueryHandler : IQueryHandler<GetParentCategoriesQuery, GetParentCategoriesResponse>
{
    private readonly List<GetParentCategoriesResponseCategory> _categories = [];

    public async Task<Result<GetParentCategoriesResponse>> Handle(
        GetParentCategoriesQuery query,
        CancellationToken cancellationToken
    )
    {
        var response = new GetParentCategoriesResponse
        {
            Categories = new PagedResult<GetParentCategoriesResponseCategory>
            {
                Items = _categories,
                TotalCount = _categories.Count,
                PageNumber = 1,
                PageSize = 10,
            },
        };

        return await Task.FromResult(Result.Success(response));
    }

    public TestGetParentCategoriesQueryHandler AddCategory(GetParentCategoriesResponseCategory category)
    {
        _categories.Add(category);
        return this;
    }

    public void Clear() => _categories.Clear();
}
