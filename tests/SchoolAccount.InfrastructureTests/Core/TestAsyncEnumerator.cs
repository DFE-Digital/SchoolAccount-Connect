namespace SchoolAccount.InfrastructureTests.Core;

/// <summary>
/// Wraps a synchronous enumerator so EF Core can await it element by element.
/// </summary>
internal sealed class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;
    private readonly CancellationToken _cancellationToken;
 
    internal TestAsyncEnumerator(IEnumerator<T> inner, CancellationToken cancellationToken)
    {
        _inner = inner;
        _cancellationToken = cancellationToken;
    }
 
    public T Current => _inner.Current;
 
    public ValueTask<bool> MoveNextAsync()
    {
        _cancellationToken.ThrowIfCancellationRequested();
        return ValueTask.FromResult(_inner.MoveNext());
    }
 
    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }
}