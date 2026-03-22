using Serilog;

namespace StudyCards.Api.Configuration;

public static class LoggingConfiguration
{
    public static void ConfigureLogging(this IHostBuilder host)
    {
        host.UseSerilog((context, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
            configuration.Enrich.FromLogContext();

            AddOpenTelemetryToSeriLog(context, configuration);
        });
    }

    // Bit of a faf in serilog. Works out of the box with Microsoft.Extensions.Logging
    private static void AddOpenTelemetryToSeriLog(HostBuilderContext context, LoggerConfiguration configuration)
    {
        var otlpEndpoint = context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
        if (!string.IsNullOrWhiteSpace(otlpEndpoint))
        {
            configuration.WriteTo.OpenTelemetry(options =>
            {
                options.Endpoint = otlpEndpoint;

                var otlpHeaders = context.Configuration["OTEL_EXPORTER_OTLP_HEADERS"];
                if (!string.IsNullOrWhiteSpace(otlpHeaders))
                {
                    options.Headers = otlpHeaders
                        .Split(',') // 1. Split multiple headers (if you ever add more than one)
                        .Select(header => header.Split('=', 2)) // 2. Split the key and value
                        .Where(parts => parts.Length == 2) // 3. Safety check to ignore malformed strings
                        .ToDictionary(
                            parts => parts[0].Trim(), // The Key
                            parts => parts[1].Trim()  // The Value
                        );
                }

                var serviceName = context.Configuration["OTEL_SERVICE_NAME"];
                options.ResourceAttributes = new Dictionary<string, object>
                {
                    { "service.name", serviceName ?? string.Empty },
                };
            });
        }
    }
}
