using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Interfaces;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.SecretsManager;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureDatabaseServices(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var secretsManager = serviceProvider.GetRequiredService<ISecretsManager>();
        var dbConnectionString = secretsManager.GetSecret(Secrets.CosmosDbConnectionString);

        services.AddDbContextFactory<DataBaseContext>(optionsBuilder =>
            optionsBuilder
    .           UseCosmos(dbConnectionString, databaseName: "StudyCards"));

        services.AddTransient<ICardRepository, CardRepository>();

        return services;
    }
}
