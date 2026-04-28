using SchoolAccount.InfrastructureTests.Builders;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class FilterRequestExtensions
{
    public static FilterRequestBuilder Create(string field)
    {
        return new FilterRequestBuilder(field);
    }
}