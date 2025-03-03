using Bitwarden.Sdk;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StudyCards.Application.Configuration;
using StudyCards.Application.Interfaces;

namespace StudyCards.Application.SecretsManager;

public class BitwardenSecretsManager(IOptions<SecretOptions> options, IMemoryCache memoryCache) : ISecretsManager
{
    public string GetSecret(string key)
    {
        return memoryCache.GetOrCreate(key, _ => 
            Get(key), 
            new MemoryCacheEntryOptions { SlidingExpiration = new TimeSpan(1, 0, 0) }) ?? string.Empty;
    }

    private string Get(string key)
    {
        using var bitwardenClient = new BitwardenClient(new BitwardenSettings
        {
            ApiUrl = options.Value.ApiUrl,
            IdentityUrl = options.Value.IdentityUrl
        });

        var apikey = !string.IsNullOrEmpty(options.Value.ApiKey) ? options.Value.ApiKey : EnvironmentVariable.SecretsApiKey ?? throw new Exception("No Key for Secrets Manager");

        bitwardenClient.Auth.LoginAccessToken(apikey);
        var allSecrets = bitwardenClient.Secrets.List(new Guid(options.Value.OrganizationId)).Data;
        var foundSecret = allSecrets.Where(x => x.Key == key).FirstOrDefault();

        if (foundSecret == null)
        {
            throw new Exception($"Secret with key {key} not found");
        }

        return bitwardenClient.Secrets.Get(foundSecret.Id).Value;
    }
}