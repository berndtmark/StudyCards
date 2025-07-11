﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Application.Configuration;
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

        var dbConnectionString = configuration.GetConnectionString("CosmosDb")
            ?? throw new InvalidOperationException("CosmosDB connection string not found.");
        
        services.AddDbContextFactory<DataBaseContext>(optionsBuilder =>
            optionsBuilder
    .           UseCosmos(dbConnectionString, databaseName: "StudyCards"));

        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IDeckRepository, DeckRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }

    public static async Task CreateDatabaseForLocal(this IServiceProvider serviceProvider, IConfiguration configuration)
    {
        if (configuration.GetConnectionString("CosmosDb")!.Contains("localhost"))
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataBaseContext>();

            await dbContext.Database.EnsureCreatedAsync();
        }
    }
}
