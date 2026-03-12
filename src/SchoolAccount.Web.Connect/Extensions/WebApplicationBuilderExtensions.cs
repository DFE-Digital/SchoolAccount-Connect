using Azure.Monitor.OpenTelemetry.AspNetCore;

namespace SchoolAccount.Web.Connect.Extensions;

internal static class WebApplicationBuilderExtensions
{
    internal static void AddApplicationSights(this WebApplicationBuilder builder, ConfigurationManager configurationManager)
    {
        if (builder.Environment.IsEnvironment("Test"))
        {
            return;
        }

        var connectionString = configurationManager.GetConnectionString("ApplicationInsights");

        builder.Services.AddOpenTelemetry()
            .UseAzureMonitor(options =>
            {
                options.ConnectionString = connectionString;
            })
            .WithMetrics(metrics =>
            {
                metrics.AddMeter("SchoolAccount.Feedback");
            });

        builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: config =>
                config.ConnectionString = connectionString,
            configureApplicationInsightsLoggerOptions: options =>
            {
                options.TrackExceptionsAsExceptionTelemetry = true;
            });
    }
}