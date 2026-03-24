using GovUk.Frontend.AspNetCore;
using SchoolAccount.Application;
using SchoolAccount.Infrastructure;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Logging;

namespace SchoolAccount.Web.Connect;

public partial class Program
{
    public static async Task Main(string[] args)
    {
        ILogger? bootstrapLogger = null;

        try
        {
            var builder = WebApplication.CreateBuilder(args);

            using var bootstrapLoggerFactory = BootstrapLogger.Create(builder);
            bootstrapLogger = bootstrapLoggerFactory.CreateLogger("Bootstrap");

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration, bootstrapLogger);
            builder.Services.AddPresentation(builder.Configuration, builder.Environment, bootstrapLogger);

            builder.Logging.AddPresentation(builder.Configuration, builder.Environment);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseHsts();
            }

            app.UseGovUkFrontend();
            app.UseStaticFiles();

            app.UseAzureAppConfigurationIfEnabled(builder.Configuration);

            app.UseHttpsRedirection();

            app.UseSession();
            app.UseRouting();

            app.ExceptionHandlers();

            app.UseAuthentication();
            app.AddMiddleware();
            app.UseAuthorization();

            app.ConfigureAreas();
            app.StripHeaders();

            await app.RunAsync();
        }
        catch (Exception ex)
        {
            if (bootstrapLogger is not null && bootstrapLogger.IsEnabled(LogLevel.Critical))
            {
                bootstrapLogger.LogCritical(
                    "Application startup failed: {ExceptionMessage}, Type: {ExceptionType}",
                    ex.Message,
                    ex.GetType().Name
                );
            }

            await Task.Delay(2000); // Give telemetry time to flush before disposal

            throw;
        }
    }

    [LoggerMessage(
        EventId = 9001,
        Level = LogLevel.Critical,
        Message = "Application startup failed: {exceptionMessage}, Type: {exceptionType}"
    )]
    internal static partial void ApplicationStartupFailed(
        ILogger logger,
        string exceptionMessage,
        string exceptionType,
        Exception exception
    );
}
