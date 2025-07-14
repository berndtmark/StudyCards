using AutoMapper;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Mapper;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardResponse>()
            .ForMember(dest => dest.ReviewCount, src => src.MapFrom(p => p.CardReviewStatus.ReviewCount))
            .ForMember(dest => dest.NextReviewDate, src => src.MapFrom(p => p.CardReviewStatus.NextReviewDate))
            .ForMember(dest => dest.ReviewPhase, src => src.MapFrom(p => p.CardReviewStatus.CurrentPhase.ToString()));
    }
}
