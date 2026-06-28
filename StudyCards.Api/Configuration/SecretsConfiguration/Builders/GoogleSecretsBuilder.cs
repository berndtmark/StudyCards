using StudyCards.Api.Configuration.Options;
using StudyCards.Infrastructure.Secrets;
using System.Text.Json;

namespace StudyCards.Api.Configuration.SecretsConfiguration.Builders;

public class GoogleSecretsBuilder : ConfigurationSecretBuilder
{
    public override string SecretKey => Secrets.GoogleAuthOptions;

    public override IEnumerable<KeyValuePair<string, string>> WithConfiguration(string secret)
    {
        var googleAuthOptions = JsonSerializer.Deserialize<GoogleAuthOptions>(secret)!;

        yield return new KeyValuePair<string, string>("GoogleAuth:ClientId", googleAuthOptions.ClientId);
        yield return new KeyValuePair<string, string>("GoogleAuth:ClientSecret", googleAuthOptions.ClientSecret);
    }
}
