using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;
using Serilog.Context;
using static Microsoft.Extensions.Logging.LogLevel;

namespace SchoolAccount.Application.Abstractions.Behaviours;

internal static partial class LoggingBehaviour
{
    internal sealed class Command<TCommand, TResponse>(ILogger<Command<TCommand, TResponse>> logger)
        : ICommandPipelineBehavior<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(
            TCommand command,
            CommandHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            var commandName = typeof(TCommand).Name;

            LogHandlingCommand(logger, commandName);

            var result = await next();

            if (result.IsSuccess)
            {
                LogCompletedCommand(logger, commandName);
            }
            else
            {
                if (result.Error.Type is ErrorType.Validation)
                {
                    LogCompletedCommandWithValidationError(logger, commandName);
                    return result;
                }

                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    LogFailedCommand(logger, commandName);
                }
            }

            return result;
        }
    }

    internal sealed class Command<TCommand>(ILogger<Command<TCommand>> logger) : ICommandPipelineBehavior<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> Handle(
            TCommand command,
            CommandHandlerDelegate next,
            CancellationToken cancellationToken
        )
        {
            var commandName = typeof(TCommand).Name;

            LogHandlingCommand(logger, commandName);

            var result = await next();

            if (result.IsSuccess)
            {
                LogCompletedCommand(logger, commandName);
            }
            else
            {
                if (result.Error.Type is ErrorType.Validation)
                {
                    LogCompletedCommandWithValidationError(logger, commandName);
                    return result;
                }

                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    LogFailedCommand(logger, commandName);
                }
            }

            return result;
        }
    }

    internal sealed class Query<TQuery, TResponse>(ILogger<Query<TQuery, TResponse>> logger)
        : IQueryPipelineBehavior<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
        public async Task<Result<TResponse>> Handle(
            TQuery query,
            QueryHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken
        )
        {
            var queryName = typeof(TQuery).Name;

            LogProcessingQuery(logger, queryName);

            var result = await next();

            if (result.IsSuccess)
            {
                LogCompletedQuery(logger, queryName);
            }
            else
            {
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    LogFailedQuery(logger, queryName);
                }
            }

            return result;
        }
    }

    [LoggerMessage(EventId = 2001, Level = Debug, Message = "Handling {commandName}")]
    private static partial void LogHandlingCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = 2002, Level = Debug, Message = "Completed command {commandName}")]
    private static partial void LogCompletedCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = 2003, Level = Debug, Message = "Completed command {commandName} with validation error")]
    private static partial void LogCompletedCommandWithValidationError(ILogger logger, string commandName);

    [LoggerMessage(EventId = 2004, Level = LogLevel.Error, Message = "Failed command {commandName} with error")]
    private static partial void LogFailedCommand(ILogger logger, string commandName);

    [LoggerMessage(EventId = 3001, Level = Debug, Message = "Processing query {queryName}")]
    private static partial void LogProcessingQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = 3002, Level = Debug, Message = "Completed query {queryName}")]
    private static partial void LogCompletedQuery(ILogger logger, string queryName);

    [LoggerMessage(EventId = 3003, Level = LogLevel.Error, Message = "Failed query {queryName} with error")]
    private static partial void LogFailedQuery(ILogger logger, string queryName);
}
