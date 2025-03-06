using Bitwarden.Sdk;

namespace StudyCards.Application.Interfaces;

public interface ISecretClient
{
    SecretsResponse Get(params string[] keys);
}