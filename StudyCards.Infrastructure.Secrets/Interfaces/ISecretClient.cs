using Bitwarden.Sdk;

namespace StudyCards.Infrastructure.Secrets.Interfaces;

public interface ISecretClient
{
    SecretsResponse Get(params string[] keys);
}