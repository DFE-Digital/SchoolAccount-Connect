namespace SchoolAccount.Web.Connect.Extensions;

internal static class WebApplicationBuilderExtensions
{
    internal static void AddApplicationSights(this WebApplicationBuilder builder, ConfigurationManager configurationManager)
    {
        if (builder.Environment.IsEnvironment("Test"))
        {
            return;
        }
        
        builder.Logging.AddApplicationInsights(
            configureTelemetryConfiguration: (config) =>
                config.ConnectionString = configurationManager.GetConnectionString("ApplicationInsights"),
            configureApplicationInsightsLoggerOptions: (options) =>
            {
                options.TrackExceptionsAsExceptionTelemetry = true;
            }
        );
    }
}