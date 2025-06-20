using AutoMapper;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Mapper;

public class DeckProfile : Profile
{
    public DeckProfile()
    {
        CreateMap<Deck, DeckResponse>();
        CreateMap<DeckSettings, DeckSettingsResponse>();
        CreateMap<DeckReviewStatus, DeckReviewStatusResponse>();
    }
}
