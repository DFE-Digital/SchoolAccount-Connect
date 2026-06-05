using SchoolAccount.Application.Features.Categories.GetCategoryHub;

namespace SchoolAccount.Tests.Common.Builders.Categories;

public class GetCategoryHubResponseCategoryBuilder
{
    private int _id = 1;
    private string _name = "Compliance";
    private string _displayName = "Compliance";

    public static GetCategoryHubResponseCategoryBuilder AResponseCategory() => new();

    public GetCategoryHubResponseCategoryBuilder WithId(int id)
    {
        _id = id;
        return this;
    }

    public GetCategoryHubResponseCategoryBuilder WithName(string name)
    {
        _name = name;
        _displayName = name;
        return this;
    }

    public GetCategoryHubResponseCategory Build() =>
        new()
        {
            Id = _id,
            Name = _name,
            DisplayName = _displayName,
        };

    public static implicit operator GetCategoryHubResponseCategory(GetCategoryHubResponseCategoryBuilder builder) =>
        builder.Build();
}
