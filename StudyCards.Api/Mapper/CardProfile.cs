using AutoMapper;
using StudyCards.Api.Models.Response;
using StudyCards.Domain.Entities;

namespace StudyCards.Api.Mapper;

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
