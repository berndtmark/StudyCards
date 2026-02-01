using StudyCards.Api.Mapper;

namespace StudyCards.Api.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddSingleton<CardMapper>();
        services.AddSingleton<AdminMapper>();
        services.AddSingleton<DeckMapper>();

        return services;
    }
}
