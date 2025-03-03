namespace StudyCards.Application.Interfaces;

public interface ISecretsManager
{
    string GetSecret(string key);
}
