using StudyCards.Server.Mapper;

namespace StudyCards.Server.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(
            typeof(CardProfile)
        );

        return services;
    }
}
