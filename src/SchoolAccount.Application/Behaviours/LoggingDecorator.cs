using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;
using Serilog.Context;
using static Microsoft.Extensions.Logging.LogLevel;

namespace SchoolAccount.Application.Behaviours;

internal static partial class LoggingDecorator
{
    internal sealed partial class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger
    ) : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;

            LogHandlingCommand(logger, commandName);

            var result = await innerHandler.Handle(command, cancellationToken);

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

        [LoggerMessage(EventId = 2001, Level = Debug, Message = "Handling {commandName}")]
        private static partial void LogHandlingCommand(ILogger logger, string commandName);

        [LoggerMessage(EventId = 2002, Level = Debug, Message = "Completed command {commandName}")]
        private static partial void LogCompletedCommand(ILogger logger, string commandName);

        [LoggerMessage(EventId = 2004, Level = LogLevel.Error, Message = "Failed command {commandName} with error")]
        private static partial void LogFailedCommand(ILogger logger, string commandName);

        [LoggerMessage(
            EventId = 2003,
            Level = Debug,
            Message = "Completed command {commandName} with validation error"
        )]
        private static partial void LogCompletedCommandWithValidationError(ILogger logger, string commandName);
    }
}
