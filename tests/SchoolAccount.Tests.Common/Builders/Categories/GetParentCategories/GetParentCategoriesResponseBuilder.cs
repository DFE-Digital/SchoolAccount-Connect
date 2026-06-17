using SchoolAccount.Application.Common;
using SchoolAccount.Application.Features.Categories.GetParentCategories;

namespace SchoolAccount.Tests.Common.Builders.Categories.GetParentCategories;

public class GetParentCategoriesResponseBuilder
{
    private List<GetParentCategoriesResponseCategory> _categories = [];

    public static GetParentCategoriesResponseBuilder AResponse() => new();

    public GetParentCategoriesResponseBuilder WithCategories(params GetParentCategoriesResponseCategory[] categories)
    {
        _categories = [.. categories];
        return this;
    }

    public GetParentCategoriesResponse Build() =>
        new()
        {
            Categories = new PagedResult<GetParentCategoriesResponseCategory>
            {
                Items = _categories,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = _categories.Count,
            },
        };

    public static implicit operator GetParentCategoriesResponse(GetParentCategoriesResponseBuilder builder) =>
        builder.Build();
}
