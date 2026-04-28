using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class FilterableItemExtensions
{
    public static FilterableItemBuilder Create(string displayValue, string value)
    {
        return new FilterableItemBuilder(displayValue, value);
    }

    public static FilterableItemBuilder Create(string value)
    {
        return new FilterableItemBuilder(value, value);
    }
}
