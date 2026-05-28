using GovUk.Frontend.AspNetCore;
using SchoolAccount.Application;
using SchoolAccount.Infrastructure;
using SchoolAccount.Web.Connect;
using SchoolAccount.Web.Connect.Extensions;
using SchoolAccount.Web.Connect.Logging;

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
    app.MapControllers();
    app.StripHeaders();

    await app.RunAsync();
}
catch (Exception ex)
{
    if (bootstrapLogger != null && bootstrapLogger.IsEnabled(LogLevel.Critical))
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

namespace SchoolAccount.Web.Connect
{
    public partial class Program
    {
        // This partial class is used to allow the Program class to be extended in other files.
    }
}
