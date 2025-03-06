using Bitwarden.Sdk;
using Microsoft.Extensions.Options;
using StudyCards.Application.Configuration;
using StudyCards.Application.Interfaces;

namespace StudyCards.Application.SecretsManager;

public class SecretsClient(IOptions<SecretOptions> options) : ISecretClient
{
    public SecretsResponse Get(params string[] keys)
    {
        using var bitwardenClient = new BitwardenClient(new BitwardenSettings
        {
            ApiUrl = options.Value.ApiUrl,
            IdentityUrl = options.Value.IdentityUrl
        });

        var apikey = !string.IsNullOrEmpty(options.Value.ApiKey) ? options.Value.ApiKey : EnvironmentVariable.SecretsApiKey ?? throw new Exception("No Key for Secrets Manager");

        bitwardenClient.Auth.LoginAccessToken(apikey);
        var allSecrets = bitwardenClient.Secrets.List(new Guid(options.Value.OrganizationId)).Data;
        var foundSecrets = allSecrets.Where(x => keys.Contains(x.Key)).ToList();

        if (!foundSecrets.Any())
        {
            throw new Exception($"No secrets found for the provided keys: {string.Join(", ", keys)}");
        }

        var secretIds = foundSecrets.Select(x => x.Id).ToArray();
        return bitwardenClient.Secrets.GetByIds(secretIds);
    }
}
