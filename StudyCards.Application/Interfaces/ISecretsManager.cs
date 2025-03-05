namespace StudyCards.Application.Interfaces;

public interface ISecretsManager
{
    string GetSecret(string key);
    T GetSecret<T>(string key) where T : class;
    IDictionary<string, string> GetSecrets(params string[] keys);
}
