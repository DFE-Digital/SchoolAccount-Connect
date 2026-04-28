using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class FilterableExtensions
{
    public static FilterableBuilder Create(string id)
    {
        return new FilterableBuilder(id);
    }
}