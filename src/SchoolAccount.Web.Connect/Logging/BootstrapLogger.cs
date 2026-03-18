using Azure.Monitor.OpenTelemetry.Exporter;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;

namespace SchoolAccount.Web.Connect.Logging;

public sealed class BootstrapLogger : IDisposable
{
    private static ILoggerFactory? _loggerFactory;

    private BootstrapLogger() { }

    public static ILoggerFactory Create(WebApplicationBuilder builder)
    {
        var appInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];
        var seqEndpoint = builder.Configuration["Seq:Endpoint"];

        _loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                if (!string.IsNullOrEmpty(appInsightsConnectionString))
                {
                    options.AddAzureMonitorLogExporter(o =>
                    {
                        o.ConnectionString = appInsightsConnectionString;
                    });
                }

                if (!string.IsNullOrWhiteSpace(seqEndpoint))
                {
                    options.AddOtlpExporter(exporterOptions =>
                    {
                        exporterOptions.Protocol = OtlpExportProtocol.HttpProtobuf;
                        exporterOptions.Endpoint = new Uri(seqEndpoint);
                    });
                }
            });

            loggingBuilder.AddConsole();
        });

        return _loggerFactory;
    }

    public void Dispose()
    {
        _loggerFactory?.Dispose();
    }
}
