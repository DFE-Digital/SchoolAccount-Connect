using SchoolAccount.Kernel;

namespace SchoolAccount.Application.Abstractions.Messaging;

public interface IMediator
{
    Task<Result> Send(ICommand command, CancellationToken cancellationToken);

    Task<Result<TResponse>> Send<TResponse>(ICommand<TResponse> command, CancellationToken cancellationToken);

    Task<Result<TResponse>> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken);
}
