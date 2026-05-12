using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace SchoolAccount.InfrastructureTests.Core;

internal sealed class TestAsyncQueryProvider<T> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    internal TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
    {
        return new TestAsyncEnumerable<T>(expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        return new TestAsyncEnumerable<TElement>(expression);
    }

    public object Execute(Expression expression)
    {
        return _inner.Execute(expression)!;
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return _inner.Execute<TResult>(expression);
    }

    // This is the method EF Core calls internally for ExecuteAsync —
    // e.g. CountAsync, FirstOrDefaultAsync, SingleAsync, etc.
    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var resultType = typeof(TResult).GetGenericArguments().First();
        var executionResult = typeof(IQueryProvider)
            .GetMethod(nameof(Execute), 1, [typeof(Expression)])!
            .MakeGenericMethod(resultType)
            .Invoke(this, [expression]);

        return (TResult)
            typeof(Task)
                .GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(resultType)
                .Invoke(null, [executionResult])!;
    }
}
