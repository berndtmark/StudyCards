using StudyCards.Application.Interfaces;
using StudyCards.Infrastructure.Secrets.Interfaces;
using System.Text.Json;

namespace StudyCards.Infrastructure.Secrets.SecretsManager;

public class SecretsManager(ISecretClient secretClient) : ISecretsManager
{
    public virtual string GetSecret(string key)
    {
        return secretClient.Get(key).Data.FirstOrDefault()?.Value ?? string.Empty;
    }

    public virtual T GetSecret<T>(string key) where T : class
    {
        var result = GetSecret(key);
        return JsonSerializer.Deserialize<T>(result) ?? throw new InvalidOperationException($"Failed to deserialize secret with key {key} to type {typeof(T).Name}");
    }

    public virtual IDictionary<string, string> GetSecrets(params string[] keys)
    {
        return secretClient.Get(keys).Data.ToDictionary(x => x.Key, x => x.Value);
    }
}
