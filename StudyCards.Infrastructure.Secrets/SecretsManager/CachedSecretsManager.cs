using Microsoft.Extensions.Caching.Memory;
using StudyCards.Application.Interfaces;
using StudyCards.Infrastructure.Secrets.Interfaces;
using System.Text.Json;

namespace StudyCards.Infrastructure.Secrets.SecretsManager;

public class CachedSecretsManager(ISecretClient secretClient, IMemoryCache memoryCache) : SecretsManager(secretClient), ISecretsManager
{
    private readonly MemoryCacheEntryOptions cacheOptions = new MemoryCacheEntryOptions { SlidingExpiration = new TimeSpan(1, 0, 0) };

    public override string GetSecret(string key)
    {
        return memoryCache.GetOrCreate(key, _ =>
            base.GetSecret(key),
            cacheOptions) ?? string.Empty;
    }

    public override T GetSecret<T>(string key) where T : class
    {
        var result = GetSecret(key);
        return JsonSerializer.Deserialize<T>(result) ?? throw new InvalidOperationException($"Failed to deserialize secret with key {key} to type {typeof(T).Name}");
    }

    public override IDictionary<string, string> GetSecrets(params string[] keys)
    {
        var result = new Dictionary<string, string>();
        var keysToFetch = new List<string>();

        // Check cache first
        foreach (var key in keys)
        {
            if (memoryCache.TryGetValue(key, out var secretData))
            {
                result[key] = secretData!.ToString()!;
            }
            else
            {
                keysToFetch.Add(key);
            }
        }

        // Only fetch from API if we have uncached keys
        if (keysToFetch.Count > 0)
        {
            var secretsResponse = base.GetSecrets(keysToFetch.ToArray());

            // Cache and process new secrets
            foreach (var secretData in secretsResponse)
            {
                memoryCache.Set(secretData.Key, secretData.Value, cacheOptions);
                result[secretData.Key] = secretData.Value;
            }
        }

        return result;
    }
}
