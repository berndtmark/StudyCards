using StudyCards.Api.Configuration.Options;
using StudyCards.Infrastructure.Secrets;
using System.Text.Json;

namespace StudyCards.Api.Configuration.SecretsConfiguration.Builders;

public class OpenTelemetrySecretsBuilder : ConfigurationSecretBuilder
{
    public override string SecretKey => Secrets.OpenTelemetryOptions;

    public override IEnumerable<KeyValuePair<string, string>> WithConfiguration(string secret)
    {
        var openTelemetryOptions = JsonSerializer.Deserialize<OpenTelemetryOptions>(secret)!;

        yield return new KeyValuePair<string, string>("OTEL_EXPORTER_OTLP_ENDPOINT", openTelemetryOptions.Endpoint);
        yield return new KeyValuePair<string, string>("OTEL_EXPORTER_OTLP_HEADERS", openTelemetryOptions.Headers);
        yield return new KeyValuePair<string, string>("OTEL_SERVICE_NAME", openTelemetryOptions.ServiceName);
    }
}
