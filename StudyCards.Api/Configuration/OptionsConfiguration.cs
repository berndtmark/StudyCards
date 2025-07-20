using StudyCards.Application.Configuration.Options;

namespace StudyCards.Api.Configuration;

public static class OptionsConfiguration
{
    public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbOptions>(configuration.GetSection(CosmosDbOptions.Key));

        return services;
    }
}
