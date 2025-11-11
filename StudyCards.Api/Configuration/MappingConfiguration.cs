using StudyCards.Api.Mapper;

namespace StudyCards.Api.Configuration;

public static class MappingConfiguration
{
    public static IServiceCollection AddMappingConfiguration(this IServiceCollection services)
    {
        services.AddAutoMapper(cfg => {
            cfg.AddProfile<CardProfile>();
            cfg.AddProfile<DeckProfile>();
            cfg.AddProfile<AdminProfile>();
        });

        return services;
    }
}
