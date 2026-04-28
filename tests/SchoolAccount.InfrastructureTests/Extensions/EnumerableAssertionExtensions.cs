using AwesomeAssertions;
using AwesomeAssertions.Collections;

namespace SchoolAccount.InfrastructureTests.Extensions;

public static class EnumerableAssertionExtensions
{
    public static AndConstraint<SubsequentOrderingAssertions<TKey>> BeInOrder<TKey, TSource>(
        this GenericCollectionAssertions<TSource> assertions,
        Func<TSource, TKey> selector,
        bool isAscending,
        string because = "",
        params object[] becauseArgs
    )
    {
        return isAscending
            ? assertions.Subject.Select(selector).Should().BeInAscendingOrder(because, becauseArgs)
            : assertions.Subject.Select(selector).Should().BeInDescendingOrder(because, becauseArgs);
    }
}
