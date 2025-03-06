using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConnectionString = configuration.GetConnectionString("CosmosDb")
            ?? throw new InvalidOperationException("CosmosDB connection string not found.");

        services.AddDbContextFactory<DataBaseContext>(optionsBuilder =>
            optionsBuilder
    .           UseCosmos(dbConnectionString, databaseName: "StudyCards"));

        services.AddTransient<ICardRepository, CardRepository>();

        return services;
    }
}
