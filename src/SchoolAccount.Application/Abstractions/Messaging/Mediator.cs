using Microsoft.Extensions.DependencyInjection;
using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Messaging;

public class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public async Task<Result> Send(ICommand command, CancellationToken cancellationToken)
    {
        var (handler, behaviors) = Resolve(
            typeof(ICommandHandler<>).MakeGenericType(command.GetType()),
            typeof(ICommandPipelineBehavior<>).MakeGenericType(command.GetType())
        );

        CommandHandlerDelegate pipeline = () => handler.Handle((dynamic)command, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)command, next, cancellationToken);
        }

        return await pipeline();
    }

    public async Task<Result<TResponse>> Send<TResponse>(
        ICommand<TResponse> command,
        CancellationToken cancellationToken
    )
    {
        var (handler, behaviors) = Resolve(
            typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse)),
            typeof(ICommandPipelineBehavior<,>).MakeGenericType(command.GetType(), typeof(TResponse))
        );

        CommandHandlerDelegate<TResponse> pipeline = () => handler.Handle((dynamic)command, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)command, next, cancellationToken);
        }

        return await pipeline();
    }

    public async Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken)
    {
        var (handler, behaviors) = Resolve(
            typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse)),
            typeof(IQueryPipelineBehavior<,>).MakeGenericType(query.GetType(), typeof(TResponse))
        );

        QueryHandlerDelegate<TResponse> pipeline = () => handler.Handle((dynamic)query, cancellationToken);

        foreach (var behavior in behaviors)
        {
            var next = pipeline;
            pipeline = () => behavior.Handle((dynamic)query, next, cancellationToken);
        }

        return await pipeline();
    }

    private (dynamic handler, List<dynamic> behaviors) Resolve(Type handlerType, Type behaviorType) =>
        (
            serviceProvider.GetRequiredService(handlerType),
            serviceProvider.GetServices(behaviorType).Cast<dynamic>().Reverse().ToList()
        );
}
