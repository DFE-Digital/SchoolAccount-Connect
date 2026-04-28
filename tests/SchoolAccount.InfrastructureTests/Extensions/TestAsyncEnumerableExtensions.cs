using SchoolAccount.InfrastructureTests.Core;

namespace SchoolAccount.InfrastructureTests.Extensions;

internal static class TestAsyncEnumerableExtensions
{
    internal static IQueryable<T> AsTestAsyncQueryable<T>(this IEnumerable<T> source)
    {
        return new TestAsyncEnumerable<T>(source);
    }
}