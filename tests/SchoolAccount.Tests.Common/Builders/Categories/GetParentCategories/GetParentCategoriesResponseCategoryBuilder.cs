using SchoolAccount.Application.Features.Categories.GetParentCategories;

namespace SchoolAccount.Tests.Common.Builders.Categories.GetParentCategories;

public class GetParentCategoriesResponseCategoryBuilder
{
    private int _id = 1;
    private string _displayName = "Category name";
    private string? _description;

    public static GetParentCategoriesResponseCategoryBuilder AResponseCategory() => new();

    public GetParentCategoriesResponseCategoryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GetParentCategoriesResponseCategoryBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public GetParentCategoriesResponseCategoryBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    public GetParentCategoriesResponseCategory Build() =>
        new()
        {
            Id = _id,
            DisplayName = _displayName,
            Description = _description,
        };

    public static implicit operator GetParentCategoriesResponseCategory(
        GetParentCategoriesResponseCategoryBuilder builder
    ) => builder.Build();
}
