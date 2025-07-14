using StudyCards.Api.Mapper;

namespace StudyCards.Api.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(CardProfile),
            typeof(DeckProfile)
        );

        return services;
    }
}
