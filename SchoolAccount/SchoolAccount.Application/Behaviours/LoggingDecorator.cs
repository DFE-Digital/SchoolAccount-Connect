using Microsoft.Extensions.Logging;
using SchoolAccount.Application.Abstractions.Messaging;
using SchoolAccount.Kernel;
using Serilog.Context;

namespace SchoolAccount.Application.Behaviours;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger
    ) : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken)
        {
            var commandName = typeof(TCommand).Name;
            
            logger.LogDebug("Handling {CommandName}", commandName);
            
            var result = await innerHandler.Handle(command, cancellationToken);
            
            if (result.IsSuccess)
            {
                logger.LogDebug("Completed command {CommandName}", commandName);
            }
            else
            {
                if (result.Error is Validation)
                {
                    logger.LogDebug("Completed command {CommandName} with validation error", commandName);
                    
                    return result;
                }
                
                using (LogContext.PushProperty("Error", result.Error, true))
                {
                    logger.LogError("Failed command {CommandName} with error", commandName);
                }
            }
            
            return result;
        }
    }
}