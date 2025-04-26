using AutoMapper;
using StudyCards.Domain.Entities;
using StudyCards.Server.Models.Response;

namespace StudyCards.Server.Mapper;

public class CardProfile : Profile
{
    public CardProfile()
    {
        CreateMap<Card, CardResponse>();
        CreateMap<Card, CardResponseWithReviews>();
        CreateMap<CardReview, CardReviewResponse>();
    }
}
