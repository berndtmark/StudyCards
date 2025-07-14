using AutoMapper;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

public class DeckProfile : Profile
{
    public DeckProfile()
    {
        CreateMap<Deck, DeckResponse>();
        CreateMap<DeckSettings, DeckSettingsResponse>();
        CreateMap<DeckReviewStatus, DeckReviewStatusResponse>();
    }
}
