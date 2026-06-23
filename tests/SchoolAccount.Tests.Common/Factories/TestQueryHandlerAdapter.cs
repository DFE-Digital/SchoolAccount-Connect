using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;

namespace SchoolAccount.Tests.Common.Factories;

public class TestQueryHandlerAdapter<TQuery, TResponse>(TestQueryHandlerRegistry registry)
    : IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery<TResponse>
{
    public Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken)
    {
        var handler =
            registry.TryGet<TQuery, TResponse>()
            ?? throw new InvalidOperationException(
                $"No test handler registered for IQueryHandler<{typeof(TQuery).Name}, {typeof(TResponse).Name}>. "
                    + "Call fixture.HandlerRegistry.Register(handler) in your test constructor."
            );

        return handler.Handle(query, cancellationToken);
    }
}
