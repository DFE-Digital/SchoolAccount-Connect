using SchoolAccount.Application.Abstractions.Messaging;

namespace SchoolAccount.Tests.Common.Factories;

public class TestQueryHandlerRegistry
{
    private readonly Dictionary<Type, object> _handlers = new();
    private readonly HashSet<Type> _serviceTypes = [];

    public IReadOnlySet<Type> ServiceTypes => _serviceTypes;

    public void Register<TQuery, TResponse>(IQueryHandler<TQuery, TResponse> handler)
        where TQuery : IQuery<TResponse>
    {
        var serviceType = typeof(IQueryHandler<TQuery, TResponse>);
        _handlers[serviceType] = handler;
        _serviceTypes.Add(serviceType);
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
