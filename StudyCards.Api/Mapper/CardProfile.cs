using AutoMapper;
using StudyCards.Api.Mapper.Converters;
using StudyCards.Api.Models.Response;
using StudyCards.Application.Common;
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

        CreateMap(typeof(PagedResult<>), typeof(PagedResult<>))
            .ConvertUsing(typeof(PagedResultConverter<,>));
    }
}
