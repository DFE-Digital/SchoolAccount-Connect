using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.IntegrationTests.Testing;

public class TestQueryHandlerRegistry
{
    private readonly Dictionary<Type, object> _handlers = new();

    public void Register<TQuery, TResponse>(IQueryHandler<TQuery, TResponse> handler)
        where TQuery : IQuery<TResponse>
    {
        _handlers[typeof(IQueryHandler<TQuery, TResponse>)] = handler;
    }

    public IQueryHandler<TQuery, TResponse>? TryGet<TQuery, TResponse>()
        where TQuery : IQuery<TResponse>
    {
        return _handlers.TryGetValue(typeof(IQueryHandler<TQuery, TResponse>), out var h)
            ? (IQueryHandler<TQuery, TResponse>)h
            : null;
    }

    public void Clear() => _handlers.Clear();
}
