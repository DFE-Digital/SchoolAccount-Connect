using SchoolAccount.Application.Features.Dashboard;

namespace SchoolAccount.Tests.Common.Builders.Dashboard.GetDashboard;

public class GetDashboardResponseCategoryItemBuilder
{
    private long _id = 1;
    private string _displayName = "Category name";
    private string? _description;

    public static GetDashboardResponseCategoryItemBuilder AResponseCategoryItem() => new();

    public GetDashboardResponseCategoryItemBuilder WithId(long id)
    {
        _id = id;
        return this;
    }

    public GetDashboardResponseCategoryItemBuilder WithDisplayName(string displayName)
    {
        _displayName = displayName;
        return this;
    }

    public GetDashboardResponseCategoryItemBuilder WithDescription(string description)
    {
        _description = description;
        return this;
    }

    private GetDashboardResponseCategoryItem Build() =>
        new()
        {
            Id = _id,
            DisplayName = _displayName,
            Description = _description,
        };

    public static GetDashboardResponseCategoryItem[] BuildMany(
        IEnumerable<GetDashboardResponseCategoryItemBuilder> builders
    ) => builders.Select(builder => builder.Build()).ToArray();

    public static implicit operator GetDashboardResponseCategoryItem(GetDashboardResponseCategoryItemBuilder builder) =>
        builder.Build();
}
