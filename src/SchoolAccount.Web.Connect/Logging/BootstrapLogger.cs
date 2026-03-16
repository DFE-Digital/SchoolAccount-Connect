using Azure.Monitor.OpenTelemetry.Exporter;

namespace SchoolAccount.Web.Connect.Logging;

public sealed class BootstrapLogger : IDisposable
{
    private static ILoggerFactory? _loggerFactory;

    private BootstrapLogger() { }

    public static ILoggerFactory Create(string? connectionString)
    {
        _loggerFactory = LoggerFactory.Create(loggingBuilder =>
        {
            loggingBuilder.AddOpenTelemetry(options =>
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    options.AddAzureMonitorLogExporter(o =>
                    {
                        o.ConnectionString = connectionString;
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
