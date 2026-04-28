using System.Linq.Expressions;

namespace SchoolAccount.InfrastructureTests.Core;

/// <summary>
/// An in-memory IQueryable that also implements IAsyncEnumerable,
/// which is what ToListAsync iterates over.
/// </summary>
internal sealed class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    internal TestAsyncEnumerable(IEnumerable<T> enumerable)
        : base(enumerable) { }
 
    internal TestAsyncEnumerable(Expression expression)
        : base(expression) { }
 
    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator(), cancellationToken);
    }
}