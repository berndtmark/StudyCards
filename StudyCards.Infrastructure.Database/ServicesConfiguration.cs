using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StudyCards.Application.Configuration;
using StudyCards.Application.Configuration.Options;
using StudyCards.Application.Interfaces.Repositories;
using StudyCards.Application.Interfaces.UnitOfWork;
using StudyCards.Infrastructure.Database.Context;
using StudyCards.Infrastructure.Database.Repositories;

namespace StudyCards.Infrastructure.Database;

public static class ServicesConfiguration
{
    public static IServiceCollection ConfigureInfrastructureDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (OpenApiGen.IsOpenApiGeneration())
            return services;

        var dbConnectionString = configuration.GetConnectionString("cosmos-db")
            ?? throw new InvalidOperationException("cosmos-db connection string not found.");

        services.AddDbContextFactory<DataBaseContext>((sp, optionsBuilder) =>
        {
            var options = sp.GetRequiredService<IOptions<CosmosDbOptions>>().Value;

            optionsBuilder.UseCosmos(dbConnectionString, databaseName: "StudyCards", BuildCosmosOptions(options));
        });

        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IStatisticRepository, StatisticRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    public static async Task CreateDatabaseForLocal(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        if (configuration.GetConnectionString("cosmos-db")!.Contains("localhost"))
        {
            Console.WriteLine("Initializing CosmosDB database for local development...");

            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }
    }

    private static Action<CosmosDbContextOptionsBuilder> BuildCosmosOptions(CosmosDbOptions options)
    {
        return cosmosOptions =>
        {
            if (!string.IsNullOrWhiteSpace(options.ConnectionMode))
            {
                var mode = Enum.Parse<ConnectionMode>(options.ConnectionMode);
                cosmosOptions.ConnectionMode(mode);
            }

            if (options.LimitToEndpoint.HasValue)
            {
                cosmosOptions.LimitToEndpoint(options.LimitToEndpoint.Value);
            }
        };
    }
}
