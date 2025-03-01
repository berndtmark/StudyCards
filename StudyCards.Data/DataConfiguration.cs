using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StudyCards.Data.Context;
using StudyCards.Data.Interfaces;
using StudyCards.Data.Repositories;

namespace StudyCards.Data;

public static class DataConfiguration
{
    public static IServiceCollection ConfigureDataServices(this IServiceCollection services, string dbConnectionString)
    {
        services.AddDbContextFactory<DataBaseContext>(optionsBuilder =>
            optionsBuilder
    .           UseCosmos(dbConnectionString, databaseName: "StudyCards"));

        services.AddTransient<ICardRepository, CardRepository>();

        return services;
    }
}
